using Elderforge.Core.Server.Types;

namespace Elderforge.Core.Server.Noise;

public class TileGroup
{
    public TileGroupType Type { get; set; }

    public List<Tile> Tiles { get; set; } = new();

    public TileGroup(TileGroupType type)
    {
        Type = type;
    }


}
