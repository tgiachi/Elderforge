using System.IO;
using System.IO.Compression;
using Elderforge.Network.Interfaces.Encoders;
using Elderforge.Network.Interfaces.Messages;
using Elderforge.Network.Interfaces.Packets;
using Elderforge.Network.Packets.Base;
using Elderforge.Network.Types;
using ProtoBuf;

namespace Elderforge.Network.Encoders;

public class ProtobufEncoder : INetworkMessageEncoder
{
    public INetworkPacket Encode<TMessage>(TMessage message, NetworkMessageType messageType) where TMessage : class
    {
        using var memoryStream = new MemoryStream();

        Serializer.Serialize(memoryStream, message);

        var packet = new NetworkPacket
        {
            Payload = Compress(memoryStream.ToArray()),
            PacketType = NetworkPacketType.Compressed,
            MessageType = messageType
        };

        return packet;
    }

    public static byte[] Compress(byte[] data)
    {
        using var memoryStream = new MemoryStream();
        using (var brotliStream = new BrotliStream(memoryStream, CompressionMode.Compress))
        {
            brotliStream.Write(data, 0, data.Length);
        }

        return memoryStream.ToArray();
    }
}
