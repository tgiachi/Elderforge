using Elderforge.Core.Server.Interfaces.Scheduler;
using Elderforge.Core.Server.Types;

namespace Elderforge.Core.Server.Data.Internal;

public class GameActionResult
{
    public GameActionResultType ResultType { get; set; }

    public IGameAction Action { get; set; }


    public static GameActionResult Success => new GameActionResult
    {
        ResultType = GameActionResultType.Success
    };
}
