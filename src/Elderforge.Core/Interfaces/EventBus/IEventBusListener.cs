namespace Elderforge.Core.Interfaces.EventBus;

public interface IEventBusListener<in TEvent>
{
    Task OnMessageAsync(TEvent message);
}
