using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Elderforge.Core.Interfaces.Events;
using Elderforge.Network.Data.Internal;
using Elderforge.Network.Interfaces.Messages;

namespace Elderforge.Network.Events.Network;

public class RegisterNetworkListenerEvent<TMessage> : IElderforgeEvent where TMessage : class, INetworkMessage
{
    public Func<string, TMessage, ValueTask<IEnumerable<SessionNetworkMessage>>> Listener { get; }

    public RegisterNetworkListenerEvent(Func<string, TMessage, ValueTask<IEnumerable<SessionNetworkMessage>>> listener)
    {
        Listener = listener;
    }
}
