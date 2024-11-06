using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Elderforge.Server;

class Program
{
    private static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        var hostBuilder = Host.CreateDefaultBuilder(args);

        hostBuilder.ConfigureLogging(s => { s.ClearProviders().AddSerilog(); });

        var host = hostBuilder.Build();


        await host.RunAsync();
    }
}
