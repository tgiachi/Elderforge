using Elderforge.Core.Numerics;


namespace Elderforge.Core.Server.Data.Internal;

public readonly struct WorldDimensions
{
    public readonly int Width { get; }  // X axis
    public readonly int Height { get; } // Y axis
    public readonly int Depth { get; }  // Z axis

    public WorldDimensions(int width, int height, int depth)
    {
        Width = width;
        Height = height;
        Depth = depth;
    }

    public Vector3Int ToChunkCount(int chunkSize)
    {
        return new Vector3Int(
            (int)Math.Ceiling((float)Width / chunkSize),
            (int)Math.Ceiling((float)Height / chunkSize),
            (int)Math.Ceiling((float)Depth / chunkSize)
        );
    }
}
