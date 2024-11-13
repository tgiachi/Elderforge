using Elderforge.Server.Data.Configs;

namespace Elderforge.Server.Data;

public class ElderforgeConfig
{
    public string ServerName { get; set; }

    public GameCommandConfig GameCommandConfig { get; set; } = new();
}
