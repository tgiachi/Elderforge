using Elderforge.Core.Server.Interfaces.World;
using Elderforge.Core.Server.Noise;
using Elderforge.Shared.Chunks;
using Elderforge.Shared.Types;

namespace Elderforge.Server.WorldGenerators;

public class PerlinTerrainGenerator : ITerrainGenerator
{
    private const float NOISE_SCALE = 0.01f;
    private const int SURFACE_HEIGHT = 64;
    private const float HEIGHT_VARIATION = 32f;
    private const int DIRT_DEPTH = 5;
    private const float CAVE_THRESHOLD = 0.6f;

    private readonly FastNoiseLite noise;

    public PerlinTerrainGenerator()
    {
        noise = new FastNoiseLite();
        noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
    }

    public void GenerateChunk(ChunkEntity chunk, int worldSeed)
    {
        noise.SetSeed(worldSeed);


        int worldX = chunk.Position.X * ChunkEntity.CHUNK_SIZE;
        int worldY = chunk.Position.Y * ChunkEntity.CHUNK_SIZE;
        int worldZ = chunk.Position.Z * ChunkEntity.CHUNK_SIZE;

        for (int x = 0; x < ChunkEntity.CHUNK_SIZE; x++)
        {
            for (int z = 0; z < ChunkEntity.CHUNK_SIZE; z++)
            {
                float heightNoise = noise.GetNoise(
                    (worldX + x) * NOISE_SCALE,
                    (worldZ + z) * NOISE_SCALE
                );


                heightNoise = (heightNoise + 1) * 0.5f;


                int terrainHeight = SURFACE_HEIGHT + (int)(heightNoise * HEIGHT_VARIATION);

                for (int y = 0; y < ChunkEntity.CHUNK_SIZE; y++)
                {
                    int worldHeight = worldY + y;


                    BlockType blockType = DetermineBlockType(
                        worldHeight,
                        terrainHeight,
                        GenerateCaveNoise(worldX + x, worldHeight, worldZ + z)
                    );

                    chunk.SetBlock(x, y, z, blockType);
                }
            }
        }
    }

    private BlockType DetermineBlockType(int worldY, int terrainHeight, float caveNoise)
    {
        if (worldY > terrainHeight)
        {
            return BlockType.Air;
        }


        if (caveNoise > CAVE_THRESHOLD && worldY < terrainHeight - 5)
        {
            return BlockType.Air;
        }


        if (worldY >= terrainHeight - DIRT_DEPTH && worldY <= terrainHeight)
        {
            return BlockType.Rock; // O un altro tipo per la superficie
        }


        if (worldY < terrainHeight - DIRT_DEPTH)
        {
            float stoneNoise = noise.GetNoise(worldY * 0.1f, worldY * 0.1f);
            if (stoneNoise > 0.5f)
            {
                return BlockType.Stone;
            }

            return BlockType.Rock;
        }

        return BlockType.Stone;
    }

    private float GenerateCaveNoise(int x, int y, int z)
    {
        float caveNoise1 = noise.GetNoise(
            x * 0.05f,
            y * 0.05f,
            z * 0.05f
        );

        float caveNoise2 = noise.GetNoise(
            x * 0.1f,
            y * 0.1f,
            z * 0.1f
        );


        return (caveNoise1 + caveNoise2) * 0.5f;
    }
}
