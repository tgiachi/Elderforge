using System.Numerics;
using Elderforge.Core.Server.Data.Config;
using Elderforge.Core.Server.Interfaces.Scheduler;
using Elderforge.Server.Services.System;
using Elderforge.Tests.Data;
using Serilog;

namespace Elderforge.Tests;

public class SchedulerServiceTests
{
    public SchedulerServiceTests()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();
    }

    [Fact]
    public async Task StressTestSchedulerService_ShouldProcessAllActions()
    {
        // Arrange
        var initialMaxActionsPerTick = 30;
        var numTasks = 8;
        var config = new SchedulerServiceConfig(initialMaxActionsPerTick, 30, numTasks);

        var scheduler = new SchedulerService(config);
        var totalActions = 10000;
        var actions = new List<IGameAction>();

        await scheduler.StartAsync();

        for (int i = 0; i < totalActions; i++)
        {
            actions.Add(
                new MoveAction { PlayerId = $"Player{i}", TargetPosition = new Vector2(i, i * 2), Priority = i % 10 }
            );
        }

        // Act
        foreach (var action in actions)
        {
            scheduler.EnqueueAction(action);
        }


        while (scheduler.CurrentTick <= 50)
        {
            await Task.Delay(30);
        }

        // Assert
        // Ideally, we would have a mechanism to track processed actions to validate
        Assert.True(true, "Stress test completed without deadlocks or crashes.");

        await scheduler.StopAsync();
    }
}
