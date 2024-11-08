using Elderforge.Network.Interfaces.Messages;
using ProtoBuf;

namespace Elderforge.Network.Packets.Motd;

[ProtoContract]
public class MotdMessage : INetworkMessage
{
    [ProtoMember(1)] public string[] Lines { get; set; }

    public MotdMessage(string[] lines)
    {
        Lines = lines;
    }
}
