using System.Diagnostics;
using Elderforge.Core.Interfaces.EventBus;
using Elderforge.Core.Interfaces.Services;
using Elderforge.Core.Server.Attributes.Services;
using Elderforge.Core.Server.Data.Directories;
using Elderforge.Core.Server.Events.Engine;
using Elderforge.Core.Server.Events.Scheduler;
using Elderforge.Core.Server.Interfaces.Services.System;
using Humanizer;
using Serilog;

namespace Elderforge.Server.Services.System;

[ElderforgeService]
public class DiagnosticService
    : IDiagnosticService, IEventBusListener<EngineStartedEvent>, IEventBusListener<EngineShuttingDownEvent>
{
    private readonly ILogger _logger = Log.Logger.ForContext<DiagnosticService>();
    private readonly IEventBusService _eventBusService;

    private readonly string _pidFileName;

    private int _printCounter;

    private int _printInterval = 120;

    public DiagnosticService(IEventBusService eventBusService, DirectoriesConfig directoriesConfig)
    {
        _eventBusService = eventBusService;
        _pidFileName = Path.Combine(directoriesConfig.Root, "elderforge.pid");
    }

    public async Task StartAsync()
    {
        _eventBusService.Subscribe<EngineStartedEvent>(this);
        _eventBusService.Subscribe<EngineShuttingDownEvent>(this);

        await _eventBusService.PublishAsync(new AddSchedulerJobEvent("PrintDiagnosticInfo", TimeSpan.FromMinutes(1), PrintDiagnosticInfoAsync));
    }

    public Task StopAsync()
    {
        return Task.CompletedTask;
    }

    public Task OnEventAsync(EngineStartedEvent message)
    {
        File.WriteAllText(_pidFileName, Process.GetCurrentProcess().Id.ToString());

        return Task.CompletedTask;
    }

    private Task PrintDiagnosticInfoAsync()
    {
        using var currentProcess = Process.GetCurrentProcess();

        _logger.Information(
            "Memory usage private: {Private} Paged: {Paged} Total Threads: {Threads} PID: {Pid}",
            currentProcess.PrivateMemorySize64.Bytes(),
            currentProcess.PagedMemorySize64.Bytes(),
            currentProcess.Threads.Count,
            currentProcess.Id
        );

        _printCounter++;

        if (_printCounter % _printInterval == 0)
        {
            _logger.Information("GC Memory: {Memory}", GC.GetTotalMemory(false).Bytes());
            _printCounter = 0;
        }


        return Task.CompletedTask;
    }

    public Task OnEventAsync(EngineShuttingDownEvent message)
    {
        if (File.Exists(_pidFileName))
        {
            File.Delete(_pidFileName);
        }

        return Task.CompletedTask;
    }
}
