using Elderforge.Core.Interfaces.Events;

namespace Elderforge.Core.Server.Events.Player;

public record PlayerLogoutEvent(Guid PlayerId, string SessionId) : IElderforgeEvent;
