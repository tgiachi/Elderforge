using Elderforge.Core.Interfaces.Services;
using Elderforge.Core.Server.Attributes.Services;
using Elderforge.Core.Server.Events.Variables;
using Elderforge.Core.Server.Interfaces.Services.System;

namespace Elderforge.Server.Services.System;


[ElderforgeService]
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
