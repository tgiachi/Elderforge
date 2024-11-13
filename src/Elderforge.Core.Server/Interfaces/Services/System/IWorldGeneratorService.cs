using Elderforge.Core.Numerics;
using Elderforge.Shared.Chunks;
using Elderforge.Shared.Types;

namespace Elderforge.Core.Server.Interfaces.Services.System;

public interface IWorldGeneratorService
{
    int WorldSeed { get; }

    ChunkEntity GenerateChunk(Vector3Int position);
    ChunkEntity GetOrGenerateChunk(Vector3Int position);

    BlockType GetBlockAt(Vector3Int worldPosition);
    void SetBlockAt(Vector3Int worldPosition, BlockType type);

    void AddChunk(ChunkEntity chunk);
    bool TryGetChunk(Vector3Int position, out ChunkEntity chunk);
    IEnumerable<ChunkEntity> GetLoadedChunks();
    void UnloadChunk(Vector3Int position);
    bool IsChunkLoaded(Vector3Int position);

    IEnumerable<Vector3Int> GetNeighboringChunkPositions(Vector3Int position);

    void GenerateWorld(IProgress<float> progress = null);
    Task GenerateWorldAsync(IProgress<float> progress = null);

    bool IsPositionInBounds(Vector3Int worldPosition);
    Vector3Int GetWorldSize();
    IEnumerable<Vector3Int> GetAllChunkPositions();
}
