using System.Collections.Concurrent;
using System.Diagnostics;
using Elderforge.Core.Interfaces.EventBus;
using Elderforge.Core.Interfaces.Services;
using Elderforge.Core.Server.Data.Directories;
using Elderforge.Core.Server.Events.Engine;
using Elderforge.Core.Server.Extensions;
using Elderforge.Core.Server.Interfaces.Services.System;
using Elderforge.Core.Server.Serialization.Map;
using Elderforge.Core.Server.Types;
using Elderforge.Core.Utils;
using Elderforge.Shared.Blocks;
using Elderforge.Shared.Chunks;
using Elderforge.Shared.Types;
using Humanizer;
using Serilog;

namespace Elderforge.Server.Services.System;

public class MapGenerationService : IMapGenerationService, IEventBusListener<EngineStartedEvent>
{
    private readonly ILogger _logger = Log.Logger.ForContext<MapGenerationService>();

    private readonly string _mapsDirectory;

    private const int chunkSize = 16;
    private const int chunkHeight = 1024;

    private readonly ConcurrentDictionary<(int, int), ChunkEntity> chunkCache = new();

    public MapGenerationService(DirectoriesConfig directoriesConfig, IEventBusService eventBusService)
    {
        _mapsDirectory = directoriesConfig[DirectoryType.Maps];
        eventBusService.Subscribe(this);
    }

    private static float Generate(int x, int z, int seed)
    {
        Random random = new Random(x * 73856093 ^ z * 19349663 ^ seed);
        return (float)random.NextDouble(); // TODO: Replace with actual Perlin Noise implementation
    }

    private ChunkEntity GenerateStaticChunk(int chunkX, int chunkZ, int seed)
    {
        _logger.Debug(
            "Generating chunk at {ChunkX}, {ChunkZ} [{ThreadId}]",
            chunkX,
            chunkZ,
            Thread.CurrentThread.ManagedThreadId
        );
        var chunk = new ChunkEntity(chunkX, chunkZ, chunkSize, chunkHeight);
        for (int x = 0; x < chunkSize; x++)
        {
            for (int z = 0; z < chunkSize; z++)
            {
                int worldX = chunkX * chunkSize + x;
                int worldZ = chunkZ * chunkSize + z;
                float heightValue = Generate(worldX, worldZ, seed) * chunkHeight;
                int height = (int)Math.Floor(heightValue);

                for (int y = 0; y < chunkHeight; y++)
                {
                    if (y < height)
                    {
                        if (y < height - 4)
                        {
                            chunk.SetBlock(x, y, z, new BlockEntity(BlockType.Stone));
                        }
                        else if (y < height - 1)
                        {
                            chunk.SetBlock(x, y, z, new BlockEntity(BlockType.Dirt));
                        }
                        else
                        {
                            chunk.SetBlock(x, y, z, new BlockEntity(BlockType.Grass));
                        }
                    }
                }
            }
        }

        return chunk;
    }

    public Task StartAsync()
    {
        return Task.CompletedTask;
    }

    public Task StopAsync()
    {
        return Task.CompletedTask;
    }

    public async Task<List<ChunkEntity>> GenerateMapAsync(int width, int height)
    {
        int numChunksX = width / chunkSize;
        int numChunksZ = height / chunkSize;
        var seed = new Random().Next();

        var chunkGenerationTasks = new List<Task>();

        for (int chunkX = 0; chunkX < numChunksX; chunkX++)
        {
            for (int chunkZ = 0; chunkZ < numChunksZ; chunkZ++)
            {
                int cx = chunkX;
                int cz = chunkZ;
                chunkGenerationTasks.Add(
                    Task.Run(
                        () =>
                        {
                            var chunk = GenerateStaticChunk(cx, cz, seed);
                            chunkCache[(cx, cz)] = chunk;
                        }
                    )
                );
            }
        }

        var startTime = Stopwatch.GetTimestamp();

        await Task.WhenAll(chunkGenerationTasks);

        var endTime = Stopwatch.GetTimestamp();


        _logger.Information(
            "Generated {ChunkCount} chunks in: {ElapsedMs}ms",
            chunkCache.Count,
            StopwatchUtils.GetElapsedMilliseconds(startTime, endTime).Milliseconds()
        );

        return chunkCache.Values.ToList();
    }

    public Task<MapSerializable> GenerateMapSerializableAsync(int width, int height, List<ChunkEntity> chunks)
    {
        var map = new MapSerializable
        {
            Width = width,
            Height = height,
            Chunks = chunks.Select(chunk => chunk.ToSerializable()).ToList()
        };

        return Task.FromResult(map);
    }

    public async Task OnEventAsync(EngineStartedEvent message)
    {
        await GenerateMapAsync(128, 128);

        // unload all chunks after generation

        var firstChunk = chunkCache.First();

        chunkCache.Clear();

        chunkCache[firstChunk.Key] = firstChunk.Value;

        GC.Collect();
    }
}
