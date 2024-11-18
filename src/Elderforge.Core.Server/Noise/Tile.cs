using Elderforge.Core.Server.Types;

namespace Elderforge.Core.Server.Noise;

public class Tile
{
    public float HeightValue { get; set; }

    public TileHeightType HeightType { get; set; }

    public MoistureType MoistureType { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    public Tile Left { get; set; }
    public Tile Right { get; set; }
    public Tile Top { get; set; }
    public Tile Bottom { get; set; }
    public int Bitmask { get; set; }

    public HeatType HeatType { get; set; }

    public bool Collidable { get; set; }

    public bool FloodFilled { get; set; }

    public Tile()
    {
    }

    public Tile(int x, int y, float heightValue)
    {
        X = x;
        Y = y;
        HeightValue = heightValue;
    }

    public void UpdateBitmask()
    {
        int count = 0;

        if (Top.HeightType == HeightType)
        {
            count += 1;
        }

        if (Right.HeightType == HeightType)
        {
            count += 2;
        }

        if (Bottom.HeightType == HeightType)
        {
            count += 4;
        }

        if (Left.HeightType == HeightType)
        {
            count += 8;
        }

        Bitmask = count;
    }
}
