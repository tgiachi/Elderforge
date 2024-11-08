using Elderforge.Core.Interfaces.Services;
using Elderforge.Network.Events;
using Elderforge.Network.Interfaces.Messages;

namespace Elderforge.Core.Server.Interfaces.Services.Game.Base;

public abstract class AbstractGameService
{
    private readonly IEventBusService _eventBusService;

    protected AbstractGameService(IEventBusService eventBusService)
    {
        _eventBusService = eventBusService;
    }

    protected void SendNetworkMessage<TMessage>(string sessionId, TMessage message) where TMessage : class, INetworkMessage
    {
        _eventBusService.Publish(new SendMessageEvent(sessionId, message));
    }

    protected void BroadcastNetworkMessage<TMessage>(TMessage message) where TMessage : class, INetworkMessage
    {
        SendNetworkMessage(string.Empty, message);
    }


    protected void SubscribeEvent<TEvent>(Action<TEvent> handler) where TEvent : class
    {
        _eventBusService.Subscribe(handler);
    }
}
