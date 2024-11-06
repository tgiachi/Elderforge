using System;
using System.Threading.Tasks;

namespace Elderforge.Network.Interfaces.Services;

public interface INetworkServer : IDisposable
{
    bool IsRunning { get; }
    Task StartAsync();

    Task StopAsync();
}
