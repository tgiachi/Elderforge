using Elderforge.Core.Interfaces.Services.Base;
using Elderforge.Core.Server.Interfaces.Scheduler;

namespace Elderforge.Core.Server.Interfaces.Services.System;

public interface ISchedulerService : IElderforgeService
{
    long CurrentTick { get; }
    void EnqueueAction(IGameAction action);
    int ActionsInQueue { get; }

    void AddSchedulerJob(string name, TimeSpan totalSpan, Func<Task> action);
}
