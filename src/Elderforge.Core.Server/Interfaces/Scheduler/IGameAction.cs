namespace Elderforge.Core.Server.Interfaces.Scheduler;

public interface IGameAction
{
    Task ExecuteAsync();
    int Priority { get; }
}
