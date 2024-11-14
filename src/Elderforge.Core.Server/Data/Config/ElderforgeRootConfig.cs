namespace Elderforge.Core.Server.Data.Config;

public class ElderforgeRootConfig
{
    public SchedulerServiceConfig Scheduler { get; set; } = new SchedulerServiceConfig(100, 50, 10);

    public WorldGeneratorConfig WorldGenerator { get; set; } = new(64);
}
