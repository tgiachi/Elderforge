using System.Runtime.Serialization;
using Elderforge.Network.Interfaces.Messages;
using Elderforge.Network.Types;

namespace Elderforge.Network.Packets.Base;

public abstract class AbstractNetworkMessage : INetworkMessage
{
    [IgnoreDataMember] public NetworkMessageType MessageType { get; }

    protected AbstractNetworkMessage(NetworkMessageType messageType)
    {
        MessageType = messageType;
    }
}
