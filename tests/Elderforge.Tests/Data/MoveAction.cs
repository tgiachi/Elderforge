using System.Numerics;
using Elderforge.Core.Server.Interfaces.Scheduler;

namespace Elderforge.Tests.Data;

public class MoveAction : IGameAction
{
    public int Priority { get; set; }
    public string PlayerId { get; set; }
    public Vector2 TargetPosition { get; set; }

    public async Task ExecuteAsync()
    {
        await Task.Run(() => Console.WriteLine($"Player {PlayerId} moves to {TargetPosition}"));
    }
}
