using Elderforge.Core.Interfaces.Events;

namespace Elderforge.Core.Server.Events.Scheduler;

public record AddSchedulerJobEvent(string Name, int TotalSeconds, Func<Task> Action) : IElderforgeEvent;
