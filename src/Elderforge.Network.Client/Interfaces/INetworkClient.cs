using System;
using Elderforge.Network.Interfaces.Messages;
using Elderforge.Network.Types;

namespace Elderforge.Network.Client.Interfaces;

public interface INetworkClient
{
    delegate void MessageReceivedEventHandler(NetworkMessageType messageType, INetworkMessage message);
    event MessageReceivedEventHandler MessageReceived;

    event EventHandler Connected;

    bool IsConnected { get; }

    void PoolEvents();

    void Connect(string host, int port);

    void SendMessage<T>(T message) where T : class, INetworkMessage;

    public IObservable<T> SubscribeToMessage<T>() where T : class, INetworkMessage;
}
