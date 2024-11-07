using Elderforge.Core.Interfaces.Services;
using Elderforge.Core.Interfaces.Services.Base;
using Elderforge.Server.Data.Services;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Elderforge.Server.HostingService;

public class AutoStartHostingService : IHostedService
{
    private readonly ILogger _logger = Log.Logger.ForContext<AutoStartHostingService>();
    private readonly IServiceProvider _serviceProvider;
    private readonly List<AutoStartService> _autoStartServices;


    public AutoStartHostingService(IServiceProvider serviceProvider, List<AutoStartService> autoStartServices)
    {
        _serviceProvider = serviceProvider;
        _autoStartServices = autoStartServices;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var serviceType in _autoStartServices.OrderBy(s => s.Priority))
        {
            _logger.Information("Starting service: {Service}", serviceType.ServiceType.Name);
            var service = _serviceProvider.GetService(serviceType.ServiceType);

            if (service is IElderforgeService elderforgeService)
            {
                await elderforgeService.StartAsync();
            }
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        foreach (var serviceType in _autoStartServices.OrderByDescending(s => s.Priority))
        {
            _logger.Information("Stopping service: {Service}", serviceType.ServiceType.Name);
            var service = _serviceProvider.GetService(serviceType.ServiceType);

            if (service is IElderforgeService elderforgeService)
            {
                await elderforgeService.StopAsync();
            }
        }
    }
}
