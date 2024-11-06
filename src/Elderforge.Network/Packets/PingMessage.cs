using System;
using Elderforge.Network.Interfaces.Messages;
using ProtoBuf;

namespace Elderforge.Network.Packets;

[ProtoContract]
public class PingMessage : INetworkMessage
{
    [ProtoMember(1)] public long Timestamp { get; set; }


    public PingMessage()
    {
        Timestamp = DateTime.UtcNow.Ticks;
    }

    public PingMessage(DateTime dateTime) => Timestamp = dateTime.Ticks;
}
