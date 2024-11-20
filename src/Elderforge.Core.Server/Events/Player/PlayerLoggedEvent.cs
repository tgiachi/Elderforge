using Elderforge.Core.Interfaces.Events;

namespace Elderforge.Core.Server.Events.Player;

public record PlayerLoggedEvent(Guid PlayerId, string SessionId) : IElderforgeEvent;
