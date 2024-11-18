using System.Numerics;
using Elderforge.Core.Interfaces.EventBus;
using Elderforge.Core.Interfaces.Services;
using Elderforge.Core.Server.Interfaces.Services.Game;
using Elderforge.Core.Server.Interfaces.Services.Game.Base;
using Elderforge.Network.Events.Sessions;
using Elderforge.Network.Interfaces.Services;

namespace Elderforge.Server.Services.Game;

public class PlayerService : AbstractGameService, IPlayerService
{
    private readonly INetworkSessionService _networkSessionService;


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
    }
}
