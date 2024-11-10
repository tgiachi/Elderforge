namespace Elderforge.Core.Server.Data.Internal;

public class SchedulerJobData
{
    public string Name { get; set; }
    public Func<Task> Action { get; set; }
    public int TotalSeconds { get; set; }
    public int CurrentSeconds { get; set; }
}
