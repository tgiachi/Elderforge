namespace Elderforge.Core.Server.Data.Config;

public record SchedulerServiceConfig(int InitialMaxActionPerTick, int TickInterval, int NumThreads);
