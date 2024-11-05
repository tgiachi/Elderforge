using Elderforge.Network.Types;

namespace Elderforge.Network.Interfaces.Packets;

public interface INetworkPacket
{
    NetworkPacketType PacketType { get; set; }
    byte[] Payload { get; set; }
    NetworkMessageType MessageType { get; set; }
}
