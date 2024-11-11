using Elderforge.Network.Interfaces.Messages;
using ProtoBuf;

namespace Elderforge.Network.Packets.System;

[ProtoContract]
public class VersionMessage : INetworkMessage
{
    public string Version { get; set; }


    public VersionMessage(string version)
    {
        Version = version;
    }

    public VersionMessage()
    {

    }
}
