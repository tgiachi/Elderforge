using Elderforge.Network.Interfaces.Messages;
using Elderforge.Shared.Types;
using ProtoBuf;

namespace Elderforge.Network.Packets.GameObjects;

[ProtoContract]
public class GameObjectActionRequestMessage : INetworkMessage
{
    [ProtoMember(1)] public string Id { get; set; }

    [ProtoMember(2)] public GameObjectActionType ActionType { get; set; }

    public GameObjectActionRequestMessage(string id, GameObjectActionType actionType)
    {
        Id = id;
        ActionType = actionType;
    }

    public GameObjectActionRequestMessage()
    {
    }
}
