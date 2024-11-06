using System;
using Elderforge.Network.Types;

namespace Elderforge.Network.Data.Internal;

public class MessageTypeObject
{
    public NetworkMessageType MessageType { get; set; }

    public Type Type { get; set; }

    public MessageTypeObject(NetworkMessageType messageType, Type type)
    {
        MessageType = messageType;
        Type = type;
    }


}
