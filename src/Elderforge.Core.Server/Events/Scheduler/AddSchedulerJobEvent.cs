using Elderforge.Core.Interfaces.Events;

namespace Elderforge.Core.Server.Events.Scheduler;

public record AddSchedulerJobEvent(string Name, TimeSpan TotalSpan, Func<Task> Action) : IElderforgeEvent;
