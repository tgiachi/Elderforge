using Elderforge.Network.Interfaces.Messages;
using ProtoBuf;

namespace Elderforge.Network.Packets.Login;

[ProtoContract]
public class LoginRequestMessage
    : INetworkMessage
{
    [ProtoMember(1)] public string Username { get; set; }


    [ProtoMember(2)] public string Password { get; set; }


    public LoginRequestMessage()
    {
    }

    public LoginRequestMessage(string username, string password)
    {
        Username = username;
        Password = password;
    }
}
