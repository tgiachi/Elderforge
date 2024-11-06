using Elderforge.Core.Extensions;
using Elderforge.Network.Data.Internal;
using Elderforge.Network.Interfaces.Services;
using Elderforge.Network.Packets;
using Elderforge.Network.Server.Data;
using Elderforge.Network.Server.Extensions;
using Elderforge.Network.Types;
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
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();

        var hostBuilder = Host.CreateDefaultBuilder(args);

        hostBuilder.ConfigureLogging(s => { s.ClearProviders().AddSerilog(); });

        hostBuilder.ConfigureServices(
            (context, services) =>
            {
                services.AddToRegisterTypedList(new MessageTypeObject(NetworkMessageType.Ping, typeof(PingMessage)));

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
