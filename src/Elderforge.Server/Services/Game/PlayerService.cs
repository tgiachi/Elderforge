using System.Collections.Concurrent;
using System.Numerics;
using Elderforge.Core.Interfaces.EventBus;
using Elderforge.Core.Interfaces.Services;
using Elderforge.Core.Server.Attributes.Services;
using Elderforge.Core.Server.Events.Player;
using Elderforge.Core.Server.GameObjects;
using Elderforge.Core.Server.Interfaces.Services.Game;
using Elderforge.Core.Server.Interfaces.Services.Game.Base;
using Elderforge.Core.Server.Interfaces.Services.System;
using Elderforge.Network.Data.Internal;
using Elderforge.Network.Events.Sessions;
using Elderforge.Network.Interfaces.Services;
using Elderforge.Network.Packets.Player;
using Elderforge.Server.Extensions;
using Serilog;

namespace Elderforge.Server.Services.Game;

[ElderforgeService]
public class PlayerService
    : AbstractGameService, IPlayerService,
        IEventBusListener<PlayerLoggedEvent>,
        IEventBusListener<PlayerLogoutEvent>
{
    private readonly INetworkSessionService _networkSessionService;

    private readonly ConcurrentDictionary<Guid, PlayerGameObject> _players = new();

    private readonly IGameObjectManagerService _gameObjectManager;

    private readonly INetworkServer _networkServer;

    private readonly ILogger _logger = Log.ForContext<IPlayerService>();


    public PlayerService(
        IEventBusService eventBusService, INetworkSessionService networkSessionService, INetworkServer networkServer,
        IGameObjectManagerService gameObjectManager
    ) : base(
        eventBusService
    )
    {
        _networkSessionService = networkSessionService;
        _networkServer = networkServer;
        _gameObjectManager = gameObjectManager;


        SubscribeEvent<SessionAddedEvent>(OnSessionAdded);
        SubscribeEvent<PlayerLoggedEvent>(this);
        SubscribeEvent<PlayerLogoutEvent>(this);
    }


    private void OnSessionAdded(SessionAddedEvent message)
    {
        var sessionObject = _networkSessionService.GetSessionObject(message.SessionId);

        sessionObject.SetPosition(new Vector3(0, 0, 0));
        sessionObject.SetRotation(new Vector3(0, 0, 0));
    }

    private void UpdatePlayer(PlayerGameObject player)
    {
        _networkSessionService.GetSessionObject(player.Id).SetPosition(player.Position);
        _networkSessionService.GetSessionObject(player.Id).SetRotation(player.Rotation);
    }

    public async Task OnEventAsync(PlayerLoggedEvent message)
    {
        _logger.Information("Player logged in: {Name}", "User" + message.SessionId);

        var player = new PlayerGameObject
        {
            Id = message.SessionId,
            Name = "User" + message.SessionId,
        };


        player.PositionSubject.Subscribe(_ => UpdatePlayer(player));
        player.RotationSubject.Subscribe(_ => UpdatePlayer(player));

        _players.TryAdd(message.PlayerId, player);

        _gameObjectManager.AddGameObject(player);
    }

    public async Task OnEventAsync(PlayerLogoutEvent message)
    {
        _logger.Information("Player logged out: {Name}", "User" + message.SessionId);

        if (_players.TryRemove(message.PlayerId, out var player))
        {
            _gameObjectManager.RemoveGameObject(player);
        }
    }
}
