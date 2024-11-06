using System;
using Elderforge.Network.Interfaces.Messages;
using Elderforge.Network.Interfaces.Packets;

namespace Elderforge.Network.Interfaces.Encoders;

public interface INetworkMessageDecoder
{
    INetworkMessage Decode(INetworkPacket packet, Type type);
}
