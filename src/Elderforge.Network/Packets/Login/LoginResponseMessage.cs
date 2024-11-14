using Elderforge.Network.Interfaces.Messages;
using ProtoBuf;

namespace Elderforge.Network.Packets.Login;

[ProtoContract]
public class LoginResponseMessage : INetworkMessage
{
    [ProtoMember(1)] public bool Success { get; set; }

    [ProtoMember(2)] public string UserId { get; set; }

    public LoginResponseMessage()
    {
    }

    public LoginResponseMessage(bool success, string userId)
    {
        Success = success;
        UserId = userId;

    }
}
