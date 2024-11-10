using System.Numerics;
using Elderforge.Core.Server.Data.Internal;
using Elderforge.Core.Server.Interfaces.Scheduler;
using Elderforge.Core.Server.Types;
using Serilog;

namespace Elderforge.Tests.Data;

public class MoveAction : IGameAction
{
    public int Priority { get; set; }
    public string PlayerId { get; set; }
    public Vector2 TargetPosition { get; set; }

    public async Task<GameActionResult> ExecuteAsync()
    {
        await Task.Run(
            () =>
            {
                //Log.Logger.Information($"Player {PlayerId} moves to {TargetPosition}");
            }
        );

        return new GameActionResult
        {
            ResultType = GameActionResultType.Success,
        };
    }
}
