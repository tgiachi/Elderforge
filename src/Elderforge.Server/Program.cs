using System.Globalization;
using CommandLine;
using Elderforge.Core.Extensions;
using Elderforge.Core.Interfaces.Services;
using Elderforge.Core.Server.Data;
using Elderforge.Core.Server.Types;
using Elderforge.Core.Services;
using Elderforge.Network.Data.Internal;
using Elderforge.Network.Interfaces.Services;
using Elderforge.Network.Packets;
using Elderforge.Network.Server.Data;
using Elderforge.Network.Server.Extensions;
using Elderforge.Network.Types;
using Elderforge.Server.Data;
using Elderforge.Server.Extensions;
using Elderforge.Server.HostingService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Elderforge.Server;

class Program
{
    private static async Task Main(string[] args)
    {
        var hostBuilder = Host.CreateApplicationBuilder(args);


        var options = Parser.Default.ParseArguments<ElderforgeServerOptions>(args)
            .WithParsed(
                s =>
                {
                    hostBuilder.Services.AddSingleton(
                        new NetworkServerConfig()
                        {
                            Port = s.Port
                        }
                    );
                }
            )
            .WithNotParsed(_ => Environment.Exit(1));


        var loggerConfiguration = new LoggerConfiguration();

        if (options.Value.IsDebug)
        {
            loggerConfiguration = loggerConfiguration.MinimumLevel.Debug();
        }
        else
        {
            loggerConfiguration.MinimumLevel.Information();
        }


        hostBuilder.Logging.ClearProviders().AddSerilog();

        if (string.IsNullOrEmpty(options.Value.RootDirectory) ||
            string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ELDERFORGE_ROOT_DIRECTORY")))
        {
            options.Value.RootDirectory = Path.Combine(Directory.GetCurrentDirectory(), "elderforge");
        }

        if (Environment.GetEnvironmentVariable("ELDERFORGE_ROOT_DIRECTORY") != null)
        {
            options.Value.RootDirectory = Environment.GetEnvironmentVariable("ELDERFORGE_ROOT_DIRECTORY");
        }

        var directoriesConfig = new DirectoriesConfig(options.Value.RootDirectory);

        Log.Logger = loggerConfiguration
            .WriteTo.Console()
            .WriteTo.File(
                Path.Combine(directoriesConfig[DirectoryType.Logs], "elderforge_server_.log"),
                rollingInterval: RollingInterval.Day,
                formatProvider: CultureInfo.InvariantCulture
            )
            .CreateLogger();


        hostBuilder.Services
            .RegisterNetworkServer<ElderforgeSession>()
            .RegisterProtobufEncoder()
            .RegisterProtobufDecoder();


        hostBuilder.Services
            .RegisterNetworkMessage<PingMessage>(NetworkMessageType.Ping);

        hostBuilder.Services.AddSingleton(directoriesConfig);


        hostBuilder.Services.AddSingleton<IEventBusService, EventBusService>();

        hostBuilder.Services.AddAutoStartService<INetworkServer>();


        hostBuilder.Services.AddHostedService<AutoStartHostingService>();

        var host = hostBuilder.Build();


        await host.StartAsync();

        await host.WaitForShutdownAsync();
    }
}
