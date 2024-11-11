using Elderforge.Shared.Types;

namespace Elderforge.Shared.Blocks;

public class BlockEntity
{
    public BlockType Type { get; set; }

    public BlockEntity(BlockType type)
    {
        Type = type;
    }

}
