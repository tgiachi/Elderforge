using Elderforge.Core.Interfaces.Services;
using Elderforge.Core.Services;
using Elderforge.Network.Client.Interfaces;

public class InstanceHolder
{
    public static INetworkClient NetworkClient { get; set; }

    public static IEventBusService EventBusService { get; set; } = new EventBusService();
}
