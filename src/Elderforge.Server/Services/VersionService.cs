using Elderforge.Core.Interfaces.Services;
using Elderforge.Core.Server.Events.Variables;
using Elderforge.Core.Server.Interfaces.Services;

namespace Elderforge.Server.Services;

public class VersionService : IVersionService
{
    public VersionService(IEventBusService eventBusService)
    {
        eventBusService.Publish(new AddVariableEvent("server_version", GetVersion()));
    }

    public string GetVersion()
    {
        // Get version from assembly
        var assembly = typeof(VersionService).Assembly;
        var version = assembly.GetName().Version;

        return version.ToString();
    }
}
