using Elderforge.Core.Server.Interfaces.World;
using Elderforge.Shared.Chunks;
using Elderforge.Shared.Types;

namespace Elderforge.Server.WorldGenerators;

public class FlatWorldGenerator : ITerrainGenerator
{
    private readonly struct Layer
    {
        public readonly int StartHeight { get; }
        public readonly int Thickness { get; }
        public readonly BlockType BlockType { get; }

        public Layer(int startHeight, int thickness, BlockType blockType)
        {
            StartHeight = startHeight;
            Thickness = thickness;
            BlockType = blockType;
        }
    }

    private readonly Layer[] layers;

    public FlatWorldGenerator()
    {
        layers = new[]
        {
            new Layer(-64, 4, BlockType.Rock), // Bottom of world
            new Layer(-60, 56, BlockType.Stone),
            new Layer(-4, 3, BlockType.Rock),
            new Layer(-1, 1, BlockType.Rock),
        };
    }

    private BlockType GetBlockTypeForHeight(int height)
    {
        if (height > 0)
        {
            return BlockType.Air;
        }


        foreach (var layer in layers)
        {
            if (height >= layer.StartHeight && height < layer.StartHeight + layer.Thickness)
            {
                return layer.BlockType;
            }
        }

        return BlockType.Rock;
    }

    public void GenerateChunk(ChunkEntity chunk, int worldSeed)
    {
        int worldY = chunk.Position.Y * ChunkEntity.CHUNK_SIZE;

        for (int x = 0; x < ChunkEntity.CHUNK_SIZE; x++)
        {
            for (int y = 0; y < ChunkEntity.CHUNK_SIZE; y++)
            {
                for (int z = 0; z < ChunkEntity.CHUNK_SIZE; z++)
                {
                    int absoluteY = worldY + y;

                    var blockType = GetBlockTypeForHeight(absoluteY);
                    chunk.SetBlock(x, y, z, blockType);
                }
            }
        }
    }
}
