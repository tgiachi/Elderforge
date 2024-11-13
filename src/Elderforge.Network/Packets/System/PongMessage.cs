using System;
using Elderforge.Network.Interfaces.Messages;
using ProtoBuf;

namespace Elderforge.Network.Packets.System;

[ProtoContract]
public class PongMessage : INetworkMessage
{
    [ProtoMember(1)] public long Timestamp { get; set; }

    public PongMessage()
    {
        Timestamp = DateTime.UtcNow.Ticks;
    }

    public PongMessage(long timestamp)
    {
        Timestamp = timestamp;
    }
}
