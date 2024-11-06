using System;
using System.Threading.Tasks;

namespace Elderforge.Network.Interfaces.Services;

public interface INetworkServer : IDisposable
{
    Task StartAsync();

    Task StopAsync();
}
