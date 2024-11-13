using Elderforge.Core.Numerics;
using Elderforge.Core.Server.Data.Config;
using Elderforge.Core.Server.Interfaces.World;
using Elderforge.Server.Services.System;
using Elderforge.Shared.Chunks;
using Elderforge.Shared.Types;

namespace Elderforge.Tests;

public class WorldGeneratorTests
{
    [Fact]
    public async Task GenerateOneChunkAsync()
    {

        var worldService = new WorldGeneratorService(new WorldGeneratorConfig
        {
            Seed = 1234,
            WorldSize = new Vector3Int(2, 2, 2)
        }, new BasicTerrainGenerator());

        var chunk = worldService.GetOrGenerateChunk(new Vector3Int(0, 0, 0));

        Assert.NotNull(chunk);

    }

    [Fact]
    public async Task GenerateWorldAsync()
    {
        var worldService = new WorldGeneratorService(new WorldGeneratorConfig
        {
            Seed = 1234,
            WorldSize = new Vector3Int(10, 10, 10)
        }, new BasicTerrainGenerator());

        await worldService.GenerateWorldAsync();

        var chunk = worldService.GetOrGenerateChunk(new Vector3Int(0, 0, 0));

        Assert.NotNull(chunk);

        var block = chunk.GetBlock(0, 0, 0);

        Assert.NotNull(block);


    }

    [Fact]
    public async Task GenerateBigWorldAndSave()
    {

        var tempFile = Path.GetTempFileName();

        var worldService = new WorldGeneratorService(new WorldGeneratorConfig
        {
            Seed = 1234,
            WorldSize = new Vector3Int(128, 128, 128)
        }, new BasicTerrainGenerator());

        await worldService.GenerateWorldAsync();


        WorldSerializer.SaveWorld(tempFile, worldService);

        Assert.True(File.Exists(tempFile));



        File.Delete(tempFile);

    }

    [Fact]
    public async Task GenerateBigWorldAndSaveCompressed()
    {

        var tempFile = Path.GetTempFileName();

        var worldService = new WorldGeneratorService(new WorldGeneratorConfig
        {
            Seed = 1234,
            WorldSize = new Vector3Int(128, 128, 128)
        }, new BasicTerrainGenerator());

        await worldService.GenerateWorldAsync();


        WorldSerializer.SaveCompressedWorld(tempFile, worldService);

        Assert.True(File.Exists(tempFile));


        File.Delete(tempFile);

    }

    [Fact]
    public async Task GenerateBigWorldAndSaveAndLoad()
    {

        var tempFile = Path.GetTempFileName();

        var worldService = new WorldGeneratorService(new WorldGeneratorConfig
        {
            Seed = 1234,
            WorldSize = new Vector3Int(128, 128, 128)
        }, new BasicTerrainGenerator());

        await worldService.GenerateWorldAsync();


        WorldSerializer.SaveWorld(tempFile, worldService);

        Assert.True(File.Exists(tempFile));


        var loadedChunks = WorldSerializer.LoadWorld(tempFile);

        Assert.NotNull(loadedChunks);

        File.Delete(tempFile);

    }

    [Fact]
    public async Task GenerateBigWorldAndSaveCompressedAndLoad()
    {

        var tempFile = Path.GetTempFileName();

        var worldService = new WorldGeneratorService(
            new WorldGeneratorConfig
            {
                Seed = 1234,
                WorldSize = new Vector3Int(128, 128, 128)
            },
            new BasicTerrainGenerator()
        );

        await worldService.GenerateWorldAsync();

        WorldSerializer.SaveCompressedWorld(tempFile, worldService);

        Assert.True(File.Exists(tempFile));

        var loadedChunks = WorldSerializer.LoadCompressedWorld(tempFile);

        Assert.NotNull(loadedChunks);

        File.Delete(tempFile);

    }
}

internal class BasicTerrainGenerator : ITerrainGenerator
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
                        type = BlockType.Air;
                    else if (value < 0.7f)
                        type = BlockType.Stone;
                    else
                        type = BlockType.Rock;

                    chunk.SetBlock(x, y, z, type);
                }
            }
        }
    }
}
