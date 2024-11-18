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

    private readonly ILogger _logger = Log.ForContext<IPlayerService>();


    public PlayerService(IEventBusService eventBusService, INetworkSessionService networkSessionService) : base(
        eventBusService
    )
    {
        _networkSessionService = networkSessionService;
        SubscribeEvent<SessionAddedEvent>(OnSessionAdded);
    }


    private void OnSessionAdded(SessionAddedEvent message)
    {
        var sessionObject = _networkSessionService.GetSessionObject(message.SessionId);

        sessionObject.SetDataObject("position", Vector3.Zero);
        sessionObject.SetDataObject("rotation", Vector3.Zero);
    }

    public async ValueTask<IEnumerable<SessionNetworkMessage>> OnMessageReceivedAsync(
        string sessionId, PlayerMoveRequestMessage message
    )
    {
        var sessionObject = _networkSessionService.GetSessionObject(sessionId);

        sessionObject.SetDataObject("position", message.Position);
        sessionObject.SetDataObject("rotation", message.Rotation);

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
