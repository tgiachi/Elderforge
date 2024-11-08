namespace Elderforge.Core.Interfaces.EventBus;

public interface IEventBusListener<in TEvent>
{
    Task OnEventAsync(TEvent message);
}
