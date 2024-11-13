using Elderforge.Core.Numerics;
using Elderforge.Shared.Blocks;

using Elderforge.Shared.Types;

namespace Elderforge.Shared.Chunks;

public class ChunkEntity
{
    public const int CHUNK_SIZE = 16;
    public Vector3Int Position { get; }
    private readonly BlockEntity[,,] blocks;

    public ChunkEntity(Vector3Int position)
    {
        Position = position;
        blocks = new BlockEntity[CHUNK_SIZE, CHUNK_SIZE, CHUNK_SIZE];
        InitializeWithAir();
    }

    public void SetBlock(int x, int y, int z, BlockType type)
    {
        if (x >= 0 && x < CHUNK_SIZE && y >= 0 && y < CHUNK_SIZE && z >= 0 && z < CHUNK_SIZE)
        {
            var worldPos = GetWorldPosition(new Vector3Int(x, y, z));
            blocks[x, y, z] = new BlockEntity(type, worldPos);
        }
    }

    public BlockEntity GetBlock(int x, int y, int z)
    {
        if (x >= 0 && x < CHUNK_SIZE && y >= 0 && y < CHUNK_SIZE && z >= 0 && z < CHUNK_SIZE)
        {
            return blocks[x, y, z];
        }

        return null;
    }

    public BlockEntity GetBlock(Vector3Int localPosition)
    {
        return GetBlock(localPosition.X, localPosition.Y, localPosition.Z);
    }

    public Vector3Int GetWorldPosition(Vector3Int localPosition)
    {
        return Position * CHUNK_SIZE + localPosition;
    }

    private void InitializeWithAir()
    {
        for (int x = 0; x < CHUNK_SIZE; x++)
        {
            for (int y = 0; y < CHUNK_SIZE; y++)
            {
                for (int z = 0; z < CHUNK_SIZE; z++)
                {
                    SetBlock(x, y, z, BlockType.Air);
                }
            }
        }
    }
}
