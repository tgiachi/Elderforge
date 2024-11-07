using Elderforge.Core.Interfaces.EventBus;

namespace Elderforge.Core.Interfaces.Services;

public interface IEventBusService
{
    Task PublishAsync<TEvent>(TEvent eventItem) where TEvent : class;
    void Publish<TEvent>(TEvent eventItem) where TEvent : class;
    IDisposable Subscribe<TEvent>(Action<TEvent> handler) where TEvent : class;
    void Subscribe<TEvent>(IEventBusListener<TEvent> listener) where TEvent : class;
}
