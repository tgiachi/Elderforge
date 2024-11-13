using Elderforge.Core.Server.Data.Internal;
using Elderforge.Core.Server.Interfaces.Scheduler;

namespace Elderforge.Core.Server.GameActions;

public class ScheduledGameAction : IGameAction
{

    public int Priority { get; } = 0;
    private Func<Task> _action;

    public ScheduledGameAction(Func<Task> action)
    {
        _action = action;
    }


    public async Task<GameActionResult> ExecuteAsync(double elapsedMilliseconds)
    {
        await _action();
        return GameActionResult.Success;

    }


}
