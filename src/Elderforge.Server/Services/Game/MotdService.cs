using Elderforge.Core.Interfaces.Services;
using Elderforge.Core.Server.Data.Motd;
using Elderforge.Core.Server.Events.Network;
using Elderforge.Core.Server.Interfaces.Services.Game;
using Elderforge.Core.Server.Interfaces.Services.Game.Base;
using Elderforge.Core.Server.Interfaces.Services.System;
using Elderforge.Network.Packets.Motd;
using Serilog;

namespace Elderforge.Server.Services.Game;

public class MotdService : AbstractGameService, IMotdService
{
    private const string MotdContextVariable = "motd";

    private readonly ILogger _logger = Log.Logger.ForContext<MotdService>();

    private readonly IScriptEngineService _scriptEngineService;

    private readonly IVariablesService _variablesService;


    public MotdService(
        IEventBusService eventBusService, IScriptEngineService scriptEngineService, IVariablesService variablesService
    ) : base(eventBusService)
    {
        _scriptEngineService = scriptEngineService;
        _variablesService = variablesService;

        SubscribeEvent<ClientConnectedEvent>(OnClientConnected);
    }

    private void OnClientConnected(ClientConnectedEvent obj)
    {
        var motd = _scriptEngineService.GetContextVariable<MotdObject>(MotdContextVariable, false);

        _logger.Debug("Sending MOTD to {sessionId}", obj.SessionId);

        if (motd == null)
        {
            _logger.Warning("No MOTD object found in script engine context, sending default MOTD");
            motd = new MotdObject(["Welcome to Elderforge!"]);
        }

        SendNetworkMessage(
            obj.SessionId,
            new MotdMessage(motd.Lines.Select(s => _variablesService.TranslateText(s)).ToArray())
        );
    }
}
