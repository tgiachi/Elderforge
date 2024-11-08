using Elderforge.Network.Interfaces.Messages;

namespace Elderforge.Network.Client.Interfaces;

public interface INetworkClient
{
    void PoolEvents();

    void Connect();

    void SendMessage<T>(T message) where T : class, INetworkMessage;
}
