using Elderforge.Core.Server.Attributes.Scripts;
using Serilog;

namespace Elderforge.Server.ScriptModules;

[ScriptModule]
public class LoggerModule
{
    private readonly ILogger _logger = Log.ForContext<LoggerModule>();


    [ScriptFunction("log_info")]
    public void LogInfo(string message)
    {
        _logger.Information(message);
    }
}
