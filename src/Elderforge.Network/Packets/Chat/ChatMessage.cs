using Elderforge.Network.Interfaces.Messages;
using ProtoBuf;

namespace Elderforge.Network.Packets.Chat;

public class ChatMessage : INetworkMessage
{
    [ProtoMember(1)] public string SourceSessionId { get; set; }

    [ProtoMember(2)] public string TargetSessionId { get; set; }

    [ProtoMember(3)] public string Message { get; set; }

    [ProtoMember(4)] public ChatMessageType Type { get; set; }


    public ChatMessage()
    {
    }

    public ChatMessage(string sourceSessionId, string targetSessionId, string message, ChatMessageType type)
    {
        SourceSessionId = sourceSessionId;
        TargetSessionId = targetSessionId;
        Message = message;
        Type = type;
    }

    public override string ToString()
    {
        return $"[{Type}] {SourceSessionId} -> {TargetSessionId}: {Message}";
    }
}
