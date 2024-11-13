using Elderforge.Core.Interfaces.Events;
using Elderforge.Core.Server.Interfaces.Scheduler;

namespace Elderforge.Core.Server.Events.Scheduler;

public record EnqueueGameActionEvent(IGameAction GameAction) : IElderforgeEvent;
