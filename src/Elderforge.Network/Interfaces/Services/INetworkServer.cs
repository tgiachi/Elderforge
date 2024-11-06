using System.Threading.Tasks;

namespace Elderforge.Network.Interfaces.Services;

public interface INetworkServer
{
    Task StartAsync();

    Task StopAsync();
}
