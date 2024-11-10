using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reactive.Linq;
using Elderforge.Core.Interfaces.EventBus;
using Elderforge.Core.Interfaces.Services;
using Elderforge.Core.Server.Data.Config;
using Elderforge.Core.Server.Events.Scheduler;
using Elderforge.Core.Server.Interfaces.Scheduler;
using Elderforge.Core.Server.Interfaces.Services.System;
using Elderforge.Core.Server.Types;
using Elderforge.Core.Utils;
using Serilog;

namespace Elderforge.Server.Services.System;

public class SchedulerService : ISchedulerService, IEventBusListener<EnqueueGameActionEvent>
{
    private readonly ILogger _logger = Log.Logger.ForContext<SchedulerService>();
    private readonly ConcurrentQueue<IGameAction> _actionQueue = new();

    private double _lastElapsedMs;

    public int ActionsInQueue => _actionQueue.Count;

    public long CurrentTick { get; private set; }

    private int _currentMaxActionsPerTick;

    private readonly SemaphoreSlim _tickLock = new(1, 1);

    private readonly SchedulerServiceConfig _config;

    private readonly ParallelOptions _parallelOptions;

    private IDisposable _tickSubscription;

    public SchedulerService(SchedulerServiceConfig config, IEventBusService eventBusService)
    {
        _config = config;
        eventBusService.Subscribe(this);

        _currentMaxActionsPerTick = _config.InitialMaxActionPerTick;

        _parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = _config.NumThreads
        };
    }

    private async Task OnTickAsync()
    {
        await _tickLock.WaitAsync();

        try
        {
            var startTime = Stopwatch.GetTimestamp();
            var actionsBatch = new List<IGameAction>();

            var processedActions = 0;

            while (processedActions < _currentMaxActionsPerTick && _actionQueue.TryDequeue(out var action))
            {
                actionsBatch.Add(action);
                processedActions++;
            }

            var sortedActions = actionsBatch.OrderByDescending(a => a.Priority).ToList();
            var successfullyProcessedActions = 0;

            await Parallel.ForEachAsync(
                sortedActions,
                _parallelOptions,
                async (action, c) =>
                {
                    try
                    {
                        var result = await action.ExecuteAsync(_lastElapsedMs);

                        if (result.ResultType == GameActionResultType.Progress ||
                            result.ResultType == GameActionResultType.Replace)
                        {
                            _actionQueue.Enqueue(action);
                        }


                        Interlocked.Increment(ref successfullyProcessedActions);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex, "Error processing action {action}, re queue ", action);
                        // requeue action
                        _actionQueue.Enqueue(action);
                    }
                }
            );

            var remainingActionsCount = 0;
            if (successfullyProcessedActions < actionsBatch.Count)
            {
                var remainingActions = sortedActions.Skip(successfullyProcessedActions).ToList();
                foreach (var action in remainingActions)
                {
                    _actionQueue.Enqueue(action);
                }

                remainingActionsCount = remainingActions.Count;
            }

            var endTime = Stopwatch.GetTimestamp();
            _lastElapsedMs = StopwatchUtils.GetElapsedMilliseconds(startTime, endTime);

            _logger.Debug(
                "Tick {currentTick} processed {processedActions} actions in {ElapsedMs}ms, {successfullyProcessedActions} successfully, remaining {remainingActions}",
                CurrentTick,
                processedActions,
                _lastElapsedMs,
                successfullyProcessedActions,
                _actionQueue.Count + remainingActionsCount
            );

            var oldMaxActionsPerTick = _currentMaxActionsPerTick;

            _currentMaxActionsPerTick = successfullyProcessedActions < _currentMaxActionsPerTick
                ? Math.Max(_config.InitialMaxActionPerTick / 2, 1)
                : _currentMaxActionsPerTick + 10;

            if (oldMaxActionsPerTick != _currentMaxActionsPerTick)
            {
                _logger.Debug(
                    "Max actions per tick changed from {oldMaxActionsPerTick} to {newMaxActionsPerTick}",
                    oldMaxActionsPerTick,
                    _currentMaxActionsPerTick
                );
            }
        }
        finally
        {
            _tickLock.Release();


            if (CurrentTick + 1 >= 1_000_000)
            {
                CurrentTick = 0;
            }


            CurrentTick++;
        }
    }


    public void EnqueueAction(IGameAction action)
    {
        _actionQueue.Enqueue(action);
    }


    public Task StartAsync()
    {
        _logger.Debug(
            "Starting scheduler service with initial actions per tick: {maxActionsPerTick}, interval: {Interval} and tasks: {NumTasks} ",
            _config.InitialMaxActionPerTick,
            _config.TickInterval,
            _config.NumThreads
        );
        _tickSubscription = Observable.Interval(TimeSpan.FromMilliseconds(_config.TickInterval))
            .Throttle(_ => Observable.FromAsync(OnTickAsync))
            .Subscribe();

        return Task.CompletedTask;
    }

    public Task StopAsync()
    {
        _tickSubscription?.Dispose();

        return Task.CompletedTask;
    }

    public Task OnEventAsync(EnqueueGameActionEvent message)
    {
        EnqueueAction(message.GameAction);

        return Task.CompletedTask;
    }
}
