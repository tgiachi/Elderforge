using System.Reflection;
using CommandLine;
using Elderforge.Core.Interfaces.Services;
using Elderforge.Core.Server.Attributes.Scripts;
using Elderforge.Core.Server.Attributes.Services;
using Elderforge.Core.Server.Data.Config;
using Elderforge.Core.Server.Data.Directories;
using Elderforge.Core.Server.Data.Internal;
using Elderforge.Core.Server.Events.Engine;
using Elderforge.Core.Server.Extensions;
using Elderforge.Core.Server.Interfaces.Services.Game;
using Elderforge.Core.Server.Interfaces.Services.System;
using Elderforge.Core.Server.Interfaces.World;
using Elderforge.Core.Server.Types;
using Elderforge.Core.Server.Utils;
using Elderforge.Core.Services;
using Elderforge.Core.Utils;
using Elderforge.Network.Client.Services;
using Elderforge.Network.Interfaces.Services;
using Elderforge.Network.Server.Data;
using Elderforge.Network.Server.Extensions;
using Elderforge.Server.Data;
using Elderforge.Server.Extensions;
using Elderforge.Server.HostingService;
using Elderforge.Server.Services.Game;
using Elderforge.Server.Services.System;
using Elderforge.Server.WorldGenerators;
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


        AssemblyUtils.GetAttribute<ScriptModuleAttribute>()
            .ForEach(
                s =>
                {
                    Log.Logger.Debug("Loading script module: {ScriptModule}", s.Name);

                    hostBuilder.Services.RegisterScriptModule(s);
                }
            );


        Log.Verbose("Registering Json settings");


        hostBuilder.Services
            .AddSingleton(JsonUtils.GetDefaultJsonSettings());


        Log.Verbose("Registering network server and encoders/decoders (Protobuf)");

        hostBuilder.Services
            .RegisterNetworkServer<ElderforgeSession>()
            .RegisterProtobufEncoder()
            .RegisterProtobufDecoder();


        Log.Verbose("Registering network messages, count: {Count}", ElderforgeInstanceHolder.MessageTypes.Count);

        ElderforgeInstanceHolder.MessageTypes.AddMessageTypesToServiceCollection(hostBuilder.Services);


        hostBuilder.Services.AddSingleton(directoriesConfig);

        Log.Verbose("Registering configs");

        hostBuilder.Services
            .AddSingleton(new SchedulerServiceConfig(100, 50, 10))
            .AddSingleton(new WorldGeneratorConfig(64))
            .AddSingleton<ITerrainGenerator, FlatWorldGenerator>()
            ;


        // hostBuilder.Services
        //     .AddAutoStartService<IEventBusService, EventBusService>()
        //     .AddAutoStartService<ISchedulerService, SchedulerService>(-1)
        //     .AddAutoStartService<IScriptEngineService, ScriptEngineService>()
        //     .AddAutoStartService<IVariablesService, VariableService>(-1)
        //     .AddAutoStartService<IDiagnosticService, DiagnosticService>()
        //     .AddAutoStartService<IVersionService, VersionService>()
        //     .AddAutoStartService<ISessionCheckService, SessionCheckService>()
        //     .AddAutoStartService<IWorldGeneratorService, WorldGeneratorService>()
        //     .AddAutoStartService<IWorldManagerService, WorldManagerService>()
        //     .AddAutoStartService<IAccountService, AccountService>()
        //     .AddAutoStartService<IGameCommandService, GameCommandService>()
        //     .AddAutoStartService<IMotdService, MotdService>()
        //     .AddAutoStartService<IGameObjectManagerService, GameObjectManagerService>()
        //     .AddAutoStartService<IPlayerService, PlayerService>()
        //     .AddAutoStartService<ITimeAndWeatherService, TimeAndWeatherService>()
        //     .AddAutoStartService<IChatService, ChatService>();


        AssemblyUtils.GetAttribute<ElderforgeServiceAttribute>()
            .ForEach(
                service =>
                {
                    var att = service.GetCustomAttribute<ElderforgeServiceAttribute>();
                    var interfaceType = AssemblyUtils.GetInterfaceOfService(service);

                    if (interfaceType == null)
                    {
                        Log.Error("Service {Service} does not have an interface", service.Name);
                        return;
                    }

                    Log.Debug(
                        "Registering service {Service} with interface {Interface}",
                        service.Name,
                        interfaceType.Name
                    );

                    hostBuilder.Services.AddAutoStartService(interfaceType, service, att.Priority);
                }
            );


        if (options.Value.DatabaseType == DatabaseType.LiteDb)
        {
            hostBuilder.Services.AddSingleton(new List<DbEntityTypeData>());

            hostBuilder.Services.AddAutoStartService<IDatabaseService, LiteDbDatabaseService>();
        }


        hostBuilder.Services.AddSingleton(options.Value);

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
