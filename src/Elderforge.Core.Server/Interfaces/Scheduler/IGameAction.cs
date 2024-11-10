using Elderforge.Core.Server.Data.Internal;

namespace Elderforge.Core.Server.Interfaces.Scheduler;

public interface IGameAction
{
    Task<GameActionResult> ExecuteAsync();
    int Priority { get; }


}
