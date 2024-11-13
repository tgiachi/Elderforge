
using Elderforge.Core.Numerics;

namespace Elderforge.Core.Server.Data.Config;

public class WorldGeneratorConfig
{
    public int Seed { get; set; }

    public Vector3Int WorldSize { get; set; }


    public WorldGeneratorConfig()
    {
        Seed = 0;
        WorldSize = new Vector3Int(0, 0, 0);
    }

    public WorldGeneratorConfig(int seed, Vector3Int worldSize)
    {
        Seed = seed;
        WorldSize = worldSize;
    }

    public WorldGeneratorConfig(int worldSize)
    {
        WorldSize = new Vector3Int(worldSize, worldSize, worldSize);
    }




}
