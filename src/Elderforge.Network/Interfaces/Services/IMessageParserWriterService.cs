using System.Threading.Tasks;
using Elderforge.Network.Packets.Base;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Elderforge.Network.Interfaces.Services;

public interface IMessageParserWriterService
{
    void ReadPackets(NetDataReader reader, NetPeer peer);

    Task WriteMessageAsync(NetPeer peer, NetDataWriter writer, NetworkPacket message);
}
