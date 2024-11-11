using Elderforge.Network.Interfaces.Messages;
using Elderforge.Network.Interfaces.Packets;
using Elderforge.Network.Types;

namespace Elderforge.Network.Interfaces.Encoders;

public interface INetworkMessageEncoder
{
    INetworkPacket Encode<TMessage>(TMessage message, NetworkMessageType messageType) where TMessage : class;
}
