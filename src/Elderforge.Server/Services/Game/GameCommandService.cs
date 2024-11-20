using Elderforge.Core.Interfaces.Services;
using Elderforge.Core.Server.Attributes.Services;
using Elderforge.Core.Server.Data.Internal;
using Elderforge.Core.Server.Interfaces.Services.Game;
using Elderforge.Core.Server.Interfaces.Services.Game.Base;
using Elderforge.Core.Server.Interfaces.Services.System;
using Elderforge.Network.Data.Internal;
using Elderforge.Network.Interfaces.Listeners;
using Elderforge.Network.Interfaces.Services;
using Elderforge.Network.Packets.Chat;
using Elderforge.Server.Data;
using Serilog;

namespace Elderforge.Server.Services.Game;


[ElderforgeService]
public class GameCommandService : AbstractGameService, IGameCommandService, INetworkMessageListener<ChatMessage>
{
    private readonly ILogger _logger = Log.ForContext<GameCommandService>();

    private readonly IScriptEngineService _scriptEngineService;

    private readonly List<(string command, GameCommandObject, Action<GameCommandContext>)> _commands = new();

    private ElderforgeConfig _gameConfig;

    public GameCommandService(
        IScriptEngineService scriptEngineService, IEventBusService eventBusService, INetworkServer networkServer
    ) : base(
        eventBusService
    )
    {
        _scriptEngineService = scriptEngineService;

        networkServer.RegisterMessageListener(this);
    }

    public Task StartAsync()
    {
        _gameConfig = _scriptEngineService.GetContextVariable<ElderforgeConfig>("gameConfig");
        _logger.Information(
            "Game command service started, trigger command key is: {key}",
            _gameConfig.GameCommandConfig.CommandSeparator
        );
        return Task.CompletedTask;
    }

    public Task StopAsync()
    {
        return Task.CompletedTask;
    }

    public void RegisterCommand(string command, string description, string help, Action<GameCommandContext> action)
    {
        _logger.Information("Registering command: {command}", command);
        _commands.Add((command, new GameCommandObject(command, description, help, null), action));
    }

    public async ValueTask<IEnumerable<SessionNetworkMessage>> OnMessageReceivedAsync(string sessionId, ChatMessage message)
    {
        if (message.Type == ChatMessageType.Command)
        {

        }

        return [];
    }
}
