using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Elderforge.Network.Data.Internal;
using Elderforge.Network.Interfaces.Messages;
using Elderforge.Network.Interfaces.Services;
using Elderforge.Network.Packets;
using Elderforge.Network.Types;
using Serilog;

namespace Elderforge.Network.Services;

public class MessageTypesService : IMessageTypesService
{
    private readonly ILogger _logger = Log.ForContext<MessageTypesService>();

    private ReadOnlyDictionary<NetworkMessageType, Type> _messageTypes = new(new Dictionary<NetworkMessageType, Type>());

    public MessageTypesService(List<MessageTypeObject>? messageTypes = null)
    {
        if (messageTypes != null)
        {
            foreach (var messageType in messageTypes)
            {
                RegisterMessageType(messageType.MessageType, messageType.Type);
            }
        }
    }


    public Type GetMessageType(NetworkMessageType messageType)
    {
        if (!_messageTypes.TryGetValue(messageType, out var type))
        {
            _logger.Error("Message type {messageType} is not registered", messageType);
            throw new ArgumentException("Message type is not registered", nameof(messageType));
        }

        return type;
    }

    public NetworkMessageType GetMessageType(Type type) => _messageTypes.FirstOrDefault(x => x.Value == type).Key;

    public void RegisterMessageType(NetworkMessageType messageType, Type type)
    {
        if (!typeof(INetworkMessage).IsAssignableFrom(type))
        {
            _logger.Error("Type {type} does not implement INetworkMessage", type.Name);
            throw new ArgumentException("Type does not implement INetworkMessage", nameof(type));
        }

        if (_messageTypes.ContainsKey(messageType))
        {
            _logger.Error("Message type {messageType} is already registered", messageType);
            throw new ArgumentException("Message type is already registered", nameof(messageType));
        }


        _logger.Debug("Registered message type {messageType} with type {type}", messageType, type.Name);

        var dictionary = new Dictionary<NetworkMessageType, Type>(_messageTypes) { { messageType, type } };

        _messageTypes = new ReadOnlyDictionary<NetworkMessageType, Type>(dictionary);
    }
}
