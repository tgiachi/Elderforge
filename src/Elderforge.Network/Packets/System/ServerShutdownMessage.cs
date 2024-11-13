using Elderforge.Network.Interfaces.Messages;
using ProtoBuf;

namespace Elderforge.Network.Packets.System;

[ProtoContract]
public class ServerShutdownMessage : INetworkMessage
{
    [ProtoMember(1)]
    public string Message { get; set; }

    public ServerShutdownMessage()
    {
    }

    public ServerShutdownMessage(string message)
    {
        Message = message;
    }
}
