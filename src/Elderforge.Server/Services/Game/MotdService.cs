using Elderforge.Core.Interfaces.Services;
using Elderforge.Core.Server.Data.Motd;
using Elderforge.Core.Server.Events.Network;
using Elderforge.Core.Server.Interfaces.Services.Game;
using Elderforge.Core.Server.Interfaces.Services.Game.Base;
using Elderforge.Core.Server.Interfaces.Services.System;
using Elderforge.Network.Data.Internal;
using Elderforge.Network.Interfaces.Listeners;
using Elderforge.Network.Interfaces.Services;
using Elderforge.Network.Packets.Motd;
using Elderforge.Network.Packets.System;
using Serilog;

namespace Elderforge.Server.Services.Game;

public class MotdService : AbstractGameService, IMotdService, INetworkMessageListener<MotdRequestMessage>
{
    private const string MotdContextVariable = "motd";

    private readonly ILogger _logger = Log.Logger.ForContext<MotdService>();
    private readonly IScriptEngineService _scriptEngineService;
    private readonly IVariablesService _variablesService;

    private readonly IVersionService _versionService;


    public MotdService(
        IEventBusService eventBusService, IScriptEngineService scriptEngineService, IVariablesService variablesService,
        IVersionService versionService, INetworkServer networkServer
    ) : base(eventBusService)
    {
        _scriptEngineService = scriptEngineService;
        _variablesService = variablesService;
        _versionService = versionService;
        networkServer.RegisterMessageListener(this);

        SubscribeEvent<ClientConnectedEvent>(OnClientConnected);
    }

    private void OnClientConnected(ClientConnectedEvent obj)
    {
        SendNetworkMessage(obj.SessionId, new VersionMessage(_versionService.GetVersion()));


        SendNetworkMessage(obj.SessionId, new ServerReadyMessage());
    }

    public async ValueTask<IEnumerable<SessionNetworkMessage>> OnMessageReceivedAsync(
        string sessionId, MotdRequestMessage message
    )
    {
        var motd = _scriptEngineService.GetContextVariable<MotdObject>(MotdContextVariable, false);

        _logger.Debug("Sending MOTD to {sessionId}", sessionId);

        if (motd == null)
        {
            _logger.Warning("No MOTD object found in script engine context, sending default MOTD");
            motd = new MotdObject(["Welcome to Elderforge!"]);
        }

        SendNetworkMessage(
            sessionId,
            new MotdMessage(motd.Lines.Select(s => _variablesService.TranslateText(s)).ToArray())
        );

        return Array.Empty<SessionNetworkMessage>();
    }
}
