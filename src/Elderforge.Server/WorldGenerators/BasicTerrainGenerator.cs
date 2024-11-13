using Elderforge.Core.Server.Interfaces.World;
using Elderforge.Shared.Chunks;
using Elderforge.Shared.Types;

namespace Elderforge.Server.WorldGenerators;

public class BasicTerrainGenerator : ITerrainGenerator
{
    public void GenerateChunk(ChunkEntity chunk, int worldSeed)
    {
        var chunkSeed = worldSeed +
                        chunk.Position.X * 73856093 +
                        chunk.Position.Y * 19349663 +
                        chunk.Position.Z * 83492791;

        Random random = new Random(chunkSeed);

        for (int x = 0; x < ChunkEntity.CHUNK_SIZE; x++)
        {
            for (int y = 0; y < ChunkEntity.CHUNK_SIZE; y++)
            {
                for (int z = 0; z < ChunkEntity.CHUNK_SIZE; z++)
                {
                    float value = (float)random.NextDouble();

                    BlockType type;
                    if (value < 0.3f)
                    {
                        type = BlockType.Air;
                    }
                    else if (value < 0.7f)
                    {
                        type = BlockType.Stone;
                    }
                    else
                    {
                        type = BlockType.Rock;
                    }

                    chunk.SetBlock(x, y, z, type);
                }
            }
        }
    }
}
