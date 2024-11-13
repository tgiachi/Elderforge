using Elderforge.Network.Interfaces.Messages;
using ProtoBuf;

namespace Elderforge.Network.Packets.System;

[ProtoContract]
public class ServerReadyMessage : INetworkMessage
{
    [ProtoMember(1)] public bool Ready { get; set; }

    public ServerReadyMessage()
    {
    }
}
