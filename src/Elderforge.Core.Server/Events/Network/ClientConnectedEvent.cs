using Elderforge.Core.Interfaces.Events;

namespace Elderforge.Core.Server.Events.Network;

public record ClientConnectedEvent(string SessionId) : IElderforgeEvent;


