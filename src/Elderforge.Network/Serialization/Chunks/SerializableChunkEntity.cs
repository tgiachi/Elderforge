using System.Collections.Generic;
using Elderforge.Network.Serialization.Blocks;
using Elderforge.Network.Serialization.Numerics;
using Elderforge.Shared.Chunks;
using Elderforge.Shared.Types;
using ProtoBuf;

namespace Elderforge.Network.Serialization.Chunks;

[ProtoContract]
public class SerializableChunkEntity
{
    [ProtoMember(1)] public SerializableVector3Int Position { get; set; }

    [ProtoMember(2)] public List<SerializableBlockEntity> Blocks { get; set; }

    // Costruttore vuoto necessario per protobuf-net
    public SerializableChunkEntity()
    {
        Blocks = new List<SerializableBlockEntity>();
    }

    public SerializableChunkEntity(ChunkEntity chunk)
    {
        Position = new SerializableVector3Int(chunk.Position);
        Blocks = new List<SerializableBlockEntity>();

        for (int x = 0; x < ChunkEntity.CHUNK_SIZE; x++)
        {
            for (int y = 0; y < ChunkEntity.CHUNK_SIZE; y++)
            {
                for (int z = 0; z < ChunkEntity.CHUNK_SIZE; z++)
                {
                    var block = chunk.GetBlock(x, y, z);
                    if (block != null && block.Type != BlockType.Air)
                    {
                        Blocks.Add(new SerializableBlockEntity(block));
                    }
                }
            }
        }
    }

    public ChunkEntity ToChunkEntity()
    {
        var chunk = new ChunkEntity(Position.ToVector3Int());


        foreach (var block in Blocks)
        {
            var worldPos = block.Position.ToVector3Int();
            var localPos = worldPos.GetLocalPosition(ChunkEntity.CHUNK_SIZE);
            chunk.SetBlock(localPos.X, localPos.Y, localPos.Z, block.Type);
        }

        return chunk;
    }
}
