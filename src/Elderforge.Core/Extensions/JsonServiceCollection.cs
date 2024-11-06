using Elderforge.Core.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Elderforge.Core.Extensions;

public static class JsonServiceCollection
{
    public static IServiceCollection RegisterDefaultJsonOptions(this IServiceCollection services)
    {
        return services.AddSingleton(JsonUtils.GetDefaultJsonSettings());
    }
}
