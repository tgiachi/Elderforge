using System;
using System.Collections.Generic;
using System.Threading.Channels;
using System.Threading.Tasks;
using Elderforge.Network.Data.Internal;
using Elderforge.Network.Interfaces.Listeners;
using Elderforge.Network.Interfaces.Messages;

namespace Elderforge.Network.Interfaces.Services;

public interface IMessageDispatcherService : IDisposable
{
    void RegisterMessageListener<TMessage>(INetworkMessageListener<TMessage> listener)
        where TMessage : class, INetworkMessage;

    void RegisterMessageListener<TMessage>(Func<string, TMessage, ValueTask<IEnumerable<SessionNetworkMessage>>> listener)
        where TMessage : class, INetworkMessage;

    void DispatchMessage<TMessage>(string sessionId, TMessage message) where TMessage : class, INetworkMessage;

    void SetOutgoingMessagesChannel(ChannelWriter<SessionNetworkPacket> outgoingMessages);
}
