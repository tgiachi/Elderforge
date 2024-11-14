using Elderforge.Core.Extensions;
using Elderforge.Network.Data.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Elderforge.Core.Server.Extensions;

public static class MessageTypeObjectExtension
{
    public static void AddMessageTypesToServiceCollection(this IEnumerable<MessageTypeObject> messageTypes, IServiceCollection services)
    {
        messageTypes.ToList().ForEach(x => services.AddToRegisterTypedList(x));
    }
}
