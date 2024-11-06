using Elderforge.Network.Interfaces.Services;
using Serilog;

namespace Elderforge.Network.Server.Services;

public class NetworkService : INetworkServer
{
    private readonly ILogger _logger = Log.ForContext<NetworkService>();
}
