using Elderforge.Core.Numerics;

using Elderforge.Shared.Types;

namespace Elderforge.Shared.Blocks;

public class BlockEntity
{
    public BlockType Type { get; set; }
    public Vector3Int Position { get; set; }

    public BlockEntity(BlockType type, Vector3Int position)
    {
        Type = type;
        Position = position;
    }
}
