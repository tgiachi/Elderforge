using System.Diagnostics;
using System.Numerics;
using Elderforge.Core.Interfaces.EventBus;
using Elderforge.Core.Interfaces.Services;
using Elderforge.Core.Numerics;
using Elderforge.Core.Server.Data.Config;
using Elderforge.Core.Server.Data.Directories;
using Elderforge.Core.Server.Data.Internal;
using Elderforge.Core.Server.Events.Engine;
using Elderforge.Core.Server.Events.World;
using Elderforge.Core.Server.Interfaces.Services.System;
using Elderforge.Core.Server.Interfaces.World;
using Elderforge.Core.Server.Types;
using Elderforge.Core.Utils;
using Elderforge.Shared.Chunks;
using Elderforge.Shared.Types;
using RandomString4Net;
using Serilog;

namespace Elderforge.Server.Services.System;

public class WorldGeneratorService : IWorldGeneratorService, IEventBusListener<EngineStartedEvent>
{
    private readonly ILogger _logger = Log.ForContext<WorldGeneratorService>();

    private readonly Dictionary<Vector3Int, ChunkEntity> chunks = new();

    private readonly string _mapDirectory;

    private readonly ITerrainGenerator _terrainGenerator;

    private readonly WorldDimensions _worldDimensions;

    private readonly IEventBusService _eventBusService;

    public int WorldSeed { get; }


    public WorldGeneratorService(
        WorldGeneratorConfig config, ITerrainGenerator terrainGenerator, IEventBusService eventBusService,
        DirectoriesConfig directoriesConfig
    )
    {
        _mapDirectory = directoriesConfig[DirectoryType.Maps];
        _terrainGenerator = terrainGenerator;
        _eventBusService = eventBusService;
        WorldSeed = config.Seed;

        _worldDimensions = new WorldDimensions(config.WorldSize.X, config.WorldSize.Y, config.WorldSize.Z);

        eventBusService.Subscribe(this);
    }

    public async Task OnEventAsync(EngineStartedEvent message)
    {
        var name = RandomString.GetString(Types.ALPHABET_MIXEDCASE);

        await GenerateWorldAsync(name);
    }

    private async Task GenerateWorldAsync(string name)
    {
        var startTime = Stopwatch.GetTimestamp();
        _logger.Information(
            "Generating world with seed {Seed} size: {X}x{Y}x{Z}",
            WorldSeed,
            _worldDimensions.Width,
            _worldDimensions.Height,
            _worldDimensions.Depth
        );

        await GenerateWorldAsync(
            new Progress<float>(
                f => { _logger.Debug("Generating world: {Progress:P}", f); }
            )
        );

        var endTime = Stopwatch.GetTimestamp();

        _logger.Information("World generated in {Time} ms", StopwatchUtils.GetElapsedMilliseconds(startTime, endTime));

        await _eventBusService.PublishAsync(new WorldCreatedEvent());

        var fileName = Path.Combine(_mapDirectory, $"{name}.map.gz");

        WorldSerializer.SaveCompressedWorld(fileName, name, this);

        _logger.Information("World saved to {FileName}", fileName);

        await _eventBusService.PublishAsync(new WorldSavedEvent(fileName));
    }


    public ChunkEntity GenerateChunk(Vector3Int position)
    {
        var chunk = new ChunkEntity(position);
        _terrainGenerator.GenerateChunk(chunk, WorldSeed);
        return chunk;
    }

    public ChunkEntity GetOrGenerateChunk(Vector3Int position)
    {
        if (!chunks.TryGetValue(position, out ChunkEntity chunk))
        {
            chunk = GenerateChunk(position);
            chunks.Add(position, chunk);
        }

        return chunk;
    }

    public BlockType GetBlockAt(Vector3Int worldPosition)
    {
        var chunkPos = worldPosition.GetChunkPosition(ChunkEntity.CHUNK_SIZE);
        var localPos = worldPosition.GetLocalPosition(ChunkEntity.CHUNK_SIZE);

        var chunk = GetOrGenerateChunk(chunkPos);
        var block = chunk.GetBlock(localPos);
        return block?.Type ?? BlockType.Air;
    }

    public void SetBlockAt(Vector3Int worldPosition, BlockType type)
    {
        var chunkPos = worldPosition.GetChunkPosition(ChunkEntity.CHUNK_SIZE);
        var localPos = worldPosition.GetLocalPosition(ChunkEntity.CHUNK_SIZE);

        var chunk = GetOrGenerateChunk(chunkPos);
        chunk.SetBlock(localPos.X, localPos.Y, localPos.Z, type);
    }

    public void AddChunk(ChunkEntity chunk)
    {
        chunks[chunk.Position] = chunk;
    }

    public bool TryGetChunk(Vector3Int position, out ChunkEntity chunk)
    {
        return chunks.TryGetValue(position, out chunk);
    }

    public IEnumerable<ChunkEntity> GetLoadedChunks()
    {
        return chunks.Values;
    }

    public void UnloadChunk(Vector3Int position)
    {
        chunks.Remove(position);
    }

    public bool IsChunkLoaded(Vector3Int position)
    {
        return chunks.ContainsKey(position);
    }

    public IEnumerable<Vector3Int> GetNeighboringChunkPositions(Vector3Int position)
    {
        yield return position + Vector3Int.Right;
        yield return position + Vector3Int.Left;
        yield return position + Vector3Int.Up;
        yield return position + Vector3Int.Down;
        yield return position + Vector3Int.Forward;
        yield return position + Vector3Int.Back;
    }

    public void GenerateWorld(IProgress<float> progress = null)
    {
        var chunkCount = _worldDimensions.ToChunkCount(ChunkEntity.CHUNK_SIZE);
        int totalChunks = chunkCount.X * chunkCount.Y * chunkCount.Z;
        int currentChunk = 0;

        for (int x = 0; x < chunkCount.X; x++)
        {
            for (int y = 0; y < chunkCount.Y; y++)
            {
                for (int z = 0; z < chunkCount.Z; z++)
                {
                    var chunkPosition = new Vector3Int(x, y, z);
                    if (!IsChunkLoaded(chunkPosition))
                    {
                        GetOrGenerateChunk(chunkPosition);
                    }

                    currentChunk++;
                    progress?.Report((float)currentChunk / totalChunks);
                }
            }
        }
    }


    public bool IsPositionInBounds(Vector3Int worldPosition)
    {
        return worldPosition.X >= 0 && worldPosition.X < _worldDimensions.Width &&
               worldPosition.Y >= 0 && worldPosition.Y < _worldDimensions.Height &&
               worldPosition.Z >= 0 && worldPosition.Z < _worldDimensions.Depth;
    }

    public Task GenerateWorldAsync(IProgress<float> progress = null)
    {
        return Task.Run(() => GenerateWorld(progress));
    }

    public Vector3Int GetWorldSize()
    {
        return new Vector3Int(_worldDimensions.Width, _worldDimensions.Height, _worldDimensions.Depth);
    }

    public IEnumerable<Vector3Int> GetAllChunkPositions()
    {
        var chunkCount = _worldDimensions.ToChunkCount(ChunkEntity.CHUNK_SIZE);
        for (int x = 0; x < chunkCount.X; x++)
        {
            for (int y = 0; y < chunkCount.Y; y++)
            {
                for (int z = 0; z < chunkCount.Z; z++)
                {
                    yield return new Vector3Int(x, y, z);
                }
            }
        }
    }


    public Task StartAsync()
    {
        return Task.CompletedTask;
    }

    public Task StopAsync()
    {
        return Task.CompletedTask;
    }
}
