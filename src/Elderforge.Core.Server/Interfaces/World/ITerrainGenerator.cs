using Elderforge.Shared.Chunks;

namespace Elderforge.Core.Server.Interfaces.World;

public interface ITerrainGenerator
{
    void GenerateChunk(ChunkEntity chunk, int seed);
}
