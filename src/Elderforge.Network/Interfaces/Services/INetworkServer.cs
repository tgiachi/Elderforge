using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Elderforge.Core.Interfaces.Services;
using Elderforge.Network.Data.Internal;
using Elderforge.Network.Interfaces.Listeners;
using Elderforge.Network.Interfaces.Messages;

namespace Elderforge.Network.Interfaces.Services;

public interface INetworkServer : IDisposable, IElderforgeService
{
    bool IsRunning { get; }
    Task StartAsync();

    Task StopAsync();

    void RegisterMessageListener<TMessage>(INetworkMessageListener<TMessage> listener)
        where TMessage : class, INetworkMessage;

    void RegisterMessageListener<TMessage>(Func<string, TMessage, ValueTask<IEnumerable<SessionNetworkMessage>>> listener)
        where TMessage : class, INetworkMessage;
}
