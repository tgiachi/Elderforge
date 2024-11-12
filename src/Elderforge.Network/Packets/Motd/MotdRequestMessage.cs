using System;
using Elderforge.Network.Interfaces.Messages;
using ProtoBuf;

namespace Elderforge.Network.Packets.Motd;

[ProtoContract]
public class MotdRequestMessage : INetworkMessage
{
    [ProtoMember(1)] public long Timestamp { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

    public MotdRequestMessage()
    {
    }
}
