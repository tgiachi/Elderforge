using Elderforge.Core.Interfaces.Events;

namespace Elderforge.Network.Events.Sessions;

public class SessionAddedEvent : IElderforgeEvent
{
    public string SessionId { get; }

    public SessionAddedEvent(string sessionId)
    {
        SessionId = sessionId;
    }

    public override string ToString()
    {
        return $"SessionAddedEvent: {SessionId}";
    }


}
