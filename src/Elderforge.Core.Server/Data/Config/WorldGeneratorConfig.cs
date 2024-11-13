
using Elderforge.Core.Numerics;

namespace Elderforge.Core.Server.Data.Config;

public class WorldGeneratorConfig
{
    public int Seed { get; set; }

    public Vector3Int WorldSize { get; set; }
}
