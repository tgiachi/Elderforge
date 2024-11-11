using Elderforge.Core.Server.Serialization.Map;
using Elderforge.Shared.Blocks;
using Elderforge.Shared.Chunks;

namespace Elderforge.Core.Server.Extensions;

public static class SerializableExtension
{
    public static BlockSerializable ToSerializable(this BlockEntity block)
    {
        return new BlockSerializable
        {
            BlockEntity = block
        };
    }

    public static BlockEntity ToBlockEntity(this BlockSerializable block)
    {
        return block.BlockEntity;
    }

    public static ChunkSerializable ToSerializable(this ChunkEntity chunk)
    {
        var chunkSerializable = new ChunkSerializable(chunk.X, chunk.Z, chunk.ChunkSize, chunk.ChunkHeight);

        for (var x = 0; x < chunk.ChunkSize; x++)
        {
            for (var y = 0; y < chunk.ChunkHeight; y++)
            {
                for (var z = 0; z < chunk.ChunkSize; z++)
                {
                    chunkSerializable.SetBlock(x, y, z, chunk.GetBlock(x, y, z).ToSerializable());
                }
            }
        }

        return chunkSerializable;
    }
}
