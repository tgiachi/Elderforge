using Elderforge.Core.Interfaces.Events;

namespace Elderforge.Core.Server.Events;

public record ClientConnectedEvent(string SessionId) : IElderforgeEvent;


