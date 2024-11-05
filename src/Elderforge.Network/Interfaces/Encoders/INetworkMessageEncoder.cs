using Elderforge.Network.Interfaces.Messages;
using Elderforge.Network.Interfaces.Packets;

namespace Elderforge.Network.Interfaces.Encoders;

public interface INetworkMessageEncoder
{
    INetworkPacket Encode<TMessage>(TMessage message) where TMessage : INetworkMessage;
}
