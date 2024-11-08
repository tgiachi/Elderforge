using CommandLine;
using Elderforge.Core.Interfaces.Services;
using Elderforge.Core.Server.Data.Directories;
using Elderforge.Core.Server.Events.Engine;
using Elderforge.Core.Server.Interfaces.Services;
using Elderforge.Core.Server.Types;
using Elderforge.Core.Services;
using Elderforge.Core.Utils;
using Elderforge.Network.Interfaces.Services;
using Elderforge.Network.Packets;
using Elderforge.Network.Packets.Chat;
using Elderforge.Network.Server.Data;
using Elderforge.Network.Server.Extensions;
using Elderforge.Network.Types;
using Elderforge.Server.Data;
using Elderforge.Server.Extensions;
using Elderforge.Server.HostingService;
using Elderforge.Server.ScriptModules;
using Elderforge.Server.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;

namespace Elderforge.Server;

public class Program
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
                new CompactJsonFormatter(),
                Path.Combine(directoriesConfig[DirectoryType.Logs], "elderforge_server_.log"),
                rollingInterval: RollingInterval.Day
            )
            .CreateLogger();

        Log.Information("Root directory: {RootDirectory}", options.Value.RootDirectory);

        hostBuilder.Services
            .RegisterScriptModule<LoggerModule>()
            .RegisterScriptModule<ContextVariableModule>()
            .RegisterScriptModule<VariableServiceModule>()
            .RegisterScriptModule<ScriptModule>()
            ;

        hostBuilder.Services
            .AddSingleton(JsonUtils.GetDefaultJsonSettings());

        hostBuilder.Services
            .RegisterNetworkServer<ElderforgeSession>()
            .RegisterProtobufEncoder()
            .RegisterProtobufDecoder();


        hostBuilder.Services
            .RegisterNetworkMessage<PingMessage>(NetworkMessageType.Ping)
            .RegisterNetworkMessage<ChatMessage>(NetworkMessageType.Chat)
            ;

        hostBuilder.Services.AddSingleton(directoriesConfig);


        hostBuilder.Services
            .AddSingleton<IEventBusService, EventBusService>()
            .AddSingleton<IScriptEngineService, ScriptEngineService>()
            .AddSingleton<IVariablesService, VariableService>()
            .AddSingleton<IVersionService, VersionService>()
            .AddSingleton<IChatService, ChatService>();

        hostBuilder.Services
            .AddAutoStartService<IVariablesService>(-1)
            .AddAutoStartService<IScriptEngineService>()
            .AddAutoStartService<IChatService>()
            .AddAutoStartService<IVersionService>();


        hostBuilder.Services.AddHostedService<AutoStartHostingService>();

        var host = hostBuilder.Build();


        await host.StartAsync();

        var networkServer = host.Services.GetRequiredService<INetworkServer>();
        var scriptEngineService = host.Services.GetRequiredService<IScriptEngineService>();
        var eventBusService = host.Services.GetRequiredService<IEventBusService>();

        var canStart = await scriptEngineService.BootstrapAsync();

        if (canStart)
        {
            await networkServer.StartAsync();

            await eventBusService.PublishAsync(new EngineStartedEvent());

            await host.WaitForShutdownAsync();
        }
        else
        {
            await host.StopAsync();

            Environment.Exit(1);
        }
    }
}
