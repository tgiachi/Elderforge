using System.Diagnostics;
using Elderforge.Core.Interfaces.EventBus;
using Elderforge.Core.Interfaces.Services;
using Elderforge.Core.Server.Data.Directories;
using Elderforge.Core.Server.Events.Engine;
using Elderforge.Core.Server.Interfaces.Services.System;

namespace Elderforge.Server.Services.System;

public class DiagnosticService
    : IDiagnosticService, IEventBusListener<EngineStartedEvent>, IEventBusListener<EngineShuttingDownEvent>
{
    private readonly IEventBusService _eventBusService;
    private readonly DirectoriesConfig _directoriesConfig;
    private readonly string _pidFileName;

    public DiagnosticService(IEventBusService eventBusService, DirectoriesConfig directoriesConfig)
    {
        _eventBusService = eventBusService;
        _directoriesConfig = directoriesConfig;
        _pidFileName = Path.Combine(_directoriesConfig.Root, "elderforge.pid");
    }

    public async Task StartAsync()
    {
        _eventBusService.Subscribe<EngineStartedEvent>(this);
        _eventBusService.Subscribe<EngineShuttingDownEvent>(this);
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

    public Task OnEventAsync(EngineShuttingDownEvent message)
    {
        if (File.Exists(_pidFileName))
        {
            File.Delete(_pidFileName);
        }

        return Task.CompletedTask;
    }
}
