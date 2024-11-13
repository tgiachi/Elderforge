using System.Numerics;
using Elderforge.Core.Server.Data.Internal;
using Elderforge.Core.Server.Interfaces.Scheduler;
using Elderforge.Core.Server.Types;
using Serilog;

namespace Elderforge.Tests.Data;

public class MoveAction : IGameAction
{
    private double _elapsedMilliseconds;
    public int Priority { get; set; }
    public string PlayerId { get; set; }
    public Vector2 TargetPosition { get; set; }

    public async Task<GameActionResult> ExecuteAsync(double elapsedMilliseconds)
    {
        _elapsedMilliseconds += elapsedMilliseconds;
        await Task.Run(
            () =>
            {
                // var timespan = TimeSpan.FromMilliseconds(_elapsedMilliseconds);
                //Log.Logger.Debug($"Player {PlayerId} moved to {TargetPosition} at {timespan}");
            }
        );

        return new GameActionResult
        {
            ResultType = GameActionResultType.Success,
        };
    }
}
