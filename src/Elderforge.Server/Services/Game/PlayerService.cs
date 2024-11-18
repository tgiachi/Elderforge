using System.Numerics;
using Elderforge.Core.Interfaces.Services;
using Elderforge.Core.Server.Interfaces.Services.Game;
using Elderforge.Core.Server.Interfaces.Services.Game.Base;
using Elderforge.Network.Data.Internal;
using Elderforge.Network.Events.Sessions;
using Elderforge.Network.Interfaces.Listeners;
using Elderforge.Network.Interfaces.Services;
using Elderforge.Network.Packets.Player;
using Elderforge.Server.Extensions;
using Serilog;

namespace Elderforge.Server.Services.Game;

public class PlayerService : AbstractGameService, IPlayerService, INetworkMessageListener<PlayerMoveRequestMessage>
{
    private readonly INetworkSessionService _networkSessionService;

    private readonly INetworkServer _networkServer;

    private readonly ILogger _logger = Log.ForContext<IPlayerService>();


    public PlayerService(
        IEventBusService eventBusService, INetworkSessionService networkSessionService, INetworkServer networkServer
    ) : base(
        eventBusService
    )
    {
        _networkSessionService = networkSessionService;
        _networkServer = networkServer;

        _networkServer.RegisterMessageListener(this);

        SubscribeEvent<SessionAddedEvent>(OnSessionAdded);
    }


    private void OnSessionAdded(SessionAddedEvent message)
    {
        var sessionObject = _networkSessionService.GetSessionObject(message.SessionId);

        sessionObject.SetPosition(new Vector3(0, 0, 0));
        sessionObject.SetRotation(new Vector3(0, 0, 0));
    }

    public async ValueTask<IEnumerable<SessionNetworkMessage>> OnMessageReceivedAsync(
        string sessionId, PlayerMoveRequestMessage message
    )
    {
        var sessionObject = _networkSessionService.GetSessionObject(sessionId);

        sessionObject.SetPosition(message.Position.ToVector3());
        sessionObject.SetRotation(message.Rotation.ToVector3());

        _logger.Debug("Player {sessionId} moved to {position}", sessionId, message.Position);

        foreach (var player in _networkSessionService.GetSessionObjectCanSee(100, message.Position.ToVector3()))
        {
            if (player.Id == sessionId)
            {
                continue;
            }

            SendNetworkMessage(
                player.Id,
                new PlayerMoveResponseMessage
                {
                    Position = message.Position,
                    Rotation = message.Rotation,
                    Id = sessionId
                }
            );
        }


        return [];
    }
}
