using Elderforge.Network.Interfaces.Messages;
using Elderforge.Network.Serialization.Chunks;
using Elderforge.Network.Serialization.Numerics;
using Elderforge.Shared.Chunks;
using ProtoBuf;

namespace Elderforge.Network.Packets.World;

[ProtoContract]
public class WorldChunkResponseMessage : INetworkMessage
{
    [ProtoMember(1)] public SerializableChunkEntity Chunk { get; set; }


    [ProtoMember(2)] public SerializableVector3Int Position { get; set; }


    public WorldChunkResponseMessage(ChunkEntity chunk)
    {
        Chunk = new SerializableChunkEntity(chunk);
        Position = new SerializableVector3Int(chunk.Position);
    }

    public WorldChunkResponseMessage()
    {
    }
}
