using Elderforge.Network.Encoders;
using Elderforge.Network.Interfaces.Encoders;
using Elderforge.Network.Interfaces.Services;
using Elderforge.Network.Server.Services;
using Elderforge.Network.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Elderforge.Network.Server.Extensions;

public static class NetworkMethodExtension
{
    public static IServiceCollection RegisterNetworkServer<TSessionObject>(this IServiceCollection services)
        where TSessionObject : class
    {
        return services
                .RegisterSessionService<TSessionObject>()
                .AddSingleton<IMessageTypesService, MessageTypesService>()
                .AddSingleton<INetworkMessageFactory, NetworkMessageFactory>()
                .AddSingleton<IMessageParserWriterService, MessageParserWriterService>()
                .AddSingleton<IMessageDispatcherService, MessageDispatcherService>()
                .AddSingleton<INetworkServer, NetworkServer<TSessionObject>>()
            ;
    }

    public static IServiceCollection RegisterProtobufEncoder(this IServiceCollection services)
    {
        return services
            .AddSingleton<INetworkMessageEncoder, ProtobufEncoder>();
    }

    public static IServiceCollection RegisterProtobufDecoder(this IServiceCollection services)
    {
        return services
            .AddSingleton<INetworkMessageDecoder, ProtobufDecoder>();
    }


    public static IServiceCollection RegisterSessionService<TSessionObject>(this IServiceCollection services)
        where TSessionObject : class
    {
        return services
            .AddSingleton<INetworkSessionService<TSessionObject>, NetworkSessionService<TSessionObject>>();
    }
}
