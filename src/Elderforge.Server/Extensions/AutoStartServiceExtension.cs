using Elderforge.Core.Extensions;
using Elderforge.Server.Data.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Elderforge.Server.Extensions;

public static class AutoStartServiceExtension
{
    public static IServiceCollection AddAutoStartService<TInterface, TService>(
        this IServiceCollection services, int priority = 0
    ) where TInterface : class where TService : class, TInterface
    {
        services.AddSingleton<TInterface, TService>();
        return services.AddToRegisterTypedList(new AutoStartService(typeof(TInterface), priority));
    }
}
