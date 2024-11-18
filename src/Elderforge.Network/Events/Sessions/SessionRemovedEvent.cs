using Elderforge.Core.Interfaces.Events;

namespace Elderforge.Network.Events.Sessions;

public class SessionRemovedEvent : IElderforgeEvent
{
    public string SessionId { get; }

    public SessionRemovedEvent(string sessionId)
    {
        SessionId = sessionId;
    }
}
