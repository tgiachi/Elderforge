using Elderforge.Shared.Blocks;

namespace Elderforge.Shared.Chunks;

public class ChunkEntity
{
    public int X { get; }

    public int Z { get; }

    public BlockEntity[,,] Blocks { get; }

    public int ChunkSize { get; }

    public int ChunkHeight { get; }

    public ChunkEntity(int x, int z, int chunkSize, int chunkHeight)
    {
        X = x;
        Z = z;
        ChunkSize = chunkSize;
        ChunkHeight = chunkHeight;
        Blocks = new BlockEntity[chunkSize, chunkHeight, chunkSize];
    }

    public void SetBlock(int x, int y, int z, BlockEntity block)
    {
        if (x >= 0 && x < ChunkSize && y >= 0 && y < ChunkHeight && z >= 0 && z < ChunkSize)
        {
            Blocks[x, y, z] = block;
        }
    }

    public BlockEntity GetBlock(int x, int y, int z)
    {
        if (x >= 0 && x < ChunkSize && y >= 0 && y < ChunkHeight && z >= 0 && z < ChunkSize)
        {
            return Blocks[x, y, z];
        }

        return null;
    }
}
