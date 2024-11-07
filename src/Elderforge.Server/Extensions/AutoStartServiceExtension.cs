using Elderforge.Core.Extensions;
using Elderforge.Server.Data.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Elderforge.Server.Extensions;

public static class AutoStartServiceExtension
{
    public static IServiceCollection AddAutoStartService<TService>(this IServiceCollection services, int priority = 0)
    {
        return services.AddToRegisterTypedList(new AutoStartService(typeof(TService), priority));
    }
}
