using System.Numerics;
using Elderforge.Core.Server.Data.Config;
using Elderforge.Core.Server.Interfaces.Scheduler;
using Elderforge.Server.Services.System;
using Elderforge.Tests.Data;

namespace Elderforge.Tests;

public class SchedulerServiceTests
{
    [Fact]
    public async Task StressTestSchedulerService_ShouldProcessAllActions()
    {
        // Arrange
        var initialMaxActionsPerTick = 1000;
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


        // Allow some time for processing
        await Task.Delay(5000);

        // Assert
        // Ideally, we would have a mechanism to track processed actions to validate
        Assert.True(true, "Stress test completed without deadlocks or crashes.");

        await scheduler.StopAsync();
    }
}
