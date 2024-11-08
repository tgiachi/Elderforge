using Elderforge.Core.Interfaces.Events;

namespace Elderforge.Core.Server.Events;

public record ClientDisconnectedEvent(string SessionId) : IElderforgeEvent;
