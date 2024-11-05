using Elderforge.Network.Interfaces.Messages;
using Elderforge.Network.Interfaces.Packets;

namespace Elderforge.Network.Interfaces.Encoders;

public interface INetworkMessageDecoder
{
    TMessage Decode<TMessage>(INetworkPacket packet) where TMessage : INetworkMessage;
}
