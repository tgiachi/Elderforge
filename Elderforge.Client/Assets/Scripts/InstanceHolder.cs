using System.Collections.Generic;
using Elderforge.Core.Interfaces.Services;
using Elderforge.Core.Services;
using Elderforge.Network.Client.Interfaces;
using Elderforge.Network.Data.Internal;
using Elderforge.Network.Packets;
using Elderforge.Network.Packets.Motd;
using Elderforge.Network.Types;

public class InstanceHolder
{

    public static List<MessageTypeObject> MessageTypes { get; set; } = new()
    {
        new MessageTypeObject(NetworkMessageType.Ping, typeof(PingMessage)),
        new MessageTypeObject(NetworkMessageType.Motd, typeof(MotdMessage)),
    };
    public static INetworkClient NetworkClient { get; set; }

    public static IEventBusService EventBusService { get; set; } = new EventBusService();
}
