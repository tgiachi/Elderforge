using Elderforge.Core.Interfaces.Events;

namespace Elderforge.Core.Server.Events.Network;

public record ClientDisconnectedEvent(string SessionId) : IElderforgeEvent;
