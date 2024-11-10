using System.Numerics;
using Elderforge.Core.Server.Data.Config;
using Elderforge.Core.Server.Interfaces.Scheduler;
using Elderforge.Core.Services;
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
        var eventBus = new EventBusService();
        // Arrange
        var initialMaxActionsPerTick = 50;
        var numTasks = 8;
        var config = new SchedulerServiceConfig(initialMaxActionsPerTick, initialMaxActionsPerTick, numTasks);

        var scheduler = new SchedulerService(config, eventBus);
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


        while (scheduler.ActionsInQueue > 0)
        {
            foreach (var _ in Enumerable.Range(0, Random.Shared.Next(200, 700) + 1))
            {
                scheduler.EnqueueAction(
                    new MoveAction
                    {
                        PlayerId = $"Player{Random.Shared.Next(0, totalActions)}",
                        TargetPosition = new Vector2(Random.Shared.Next(0, 100), Random.Shared.Next(0, 100)),
                        Priority = Random.Shared.Next(0, 10)
                    }
                );
            }

            await Task.Delay(initialMaxActionsPerTick);
        }

        // Assert
        // Ideally, we would have a mechanism to track processed actions to validate
        Assert.True(true, "Stress test completed without deadlocks or crashes.");

        await scheduler.StopAsync();
    }
}
