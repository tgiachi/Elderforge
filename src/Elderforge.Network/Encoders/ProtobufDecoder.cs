using System;
using System.IO;
using System.IO.Compression;
using Elderforge.Network.Interfaces.Encoders;
using Elderforge.Network.Interfaces.Messages;
using Elderforge.Network.Interfaces.Packets;
using Elderforge.Network.Types;
using ProtoBuf;

namespace Elderforge.Network.Encoders;

public class ProtobufDecoder : INetworkMessageDecoder
{
    public INetworkMessage Decode(INetworkPacket packet, Type type)
    {
        using var stream = new MemoryStream(packet.Payload);

        if (packet.PacketType.HasFlag(NetworkPacketType.Compressed))
        {
            using var tmpStream = new MemoryStream(Decompress(packet.Payload));

            return Serializer.Deserialize(type, tmpStream) as INetworkMessage;
        }

        return Serializer.Deserialize(type, stream) as INetworkMessage;
    }

    public static byte[] Decompress(byte[] compressedData)
    {
        using var inputStream = new MemoryStream(compressedData);
        using var outputStream = new MemoryStream();
        using (var brotliStream = new BrotliStream(inputStream, CompressionMode.Decompress))
        {
            brotliStream.CopyTo(outputStream);
        }

        return outputStream.ToArray();
    }
}
