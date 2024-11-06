using Elderforge.Network.Interfaces.Services;
using Elderforge.Network.Server.Data;
using Elderforge.Network.Server.Extensions;
using Elderforge.Server.Data;
using Microsoft.Extensions.DependencyInjection;
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

        hostBuilder.ConfigureServices(
            (context, services) =>
            {
                services
                    .RegisterNetworkServer<ElderforgeSession>()
                    .RegisterProtobufEncoder()
                    .RegisterProtobufDecoder();

                services.AddSingleton(new NetworkServerConfig());
            }
        );

        var host = hostBuilder.Build();


        var server = host.Services.GetService<INetworkServer>();

        await server.StartAsync();

        await host.RunAsync();
    }
}
