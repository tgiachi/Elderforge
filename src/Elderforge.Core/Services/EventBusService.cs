using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Elderforge.Core.Interfaces.EventBus;
using Elderforge.Core.Interfaces.Services;
using Serilog;

namespace Elderforge.Core.Services;

public class EventBusService : IEventBusService
{
    private readonly ILogger _logger = Log.ForContext<EventBusService>();

    private readonly ConcurrentDictionary<Type, object> _subjects = new();

    public async Task PublishAsync<TEvent>(TEvent eventItem) where TEvent : class
    {
        if (_subjects.TryGetValue(typeof(TEvent), out var subject))
        {
            var typedSubject = (ISubject<TEvent>)subject;
            typedSubject.OnNext(eventItem);
        }
    }

    public void Publish<TEvent>(TEvent eventItem) where TEvent : class
    {
        _logger.Debug("Publishing event: {Event}", eventItem);

        if (_subjects.TryGetValue(typeof(TEvent), out var subject))
        {
            var typedSubject = (ISubject<TEvent>)subject;
            typedSubject.OnNext(eventItem);
        }
    }

    public IDisposable Subscribe<TEvent>(Action<TEvent> handler) where TEvent : class
    {
        var subject = (ISubject<TEvent>)_subjects.GetOrAdd(typeof(TEvent), _ => new Subject<TEvent>());
        return subject.AsObservable().Subscribe(handler);
    }

    public void Subscribe<TEvent>(IEventBusListener<TEvent> listener) where TEvent : class
    {
        var subject = (ISubject<TEvent>)_subjects.GetOrAdd(typeof(TEvent), _ => new Subject<TEvent>());
        subject.AsObservable().Subscribe(async e => await listener.OnEventAsync(e));
    }
}
