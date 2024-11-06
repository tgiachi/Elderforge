using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Elderforge.Network.Interfaces.Encoders;
using Elderforge.Network.Interfaces.Messages;
using Elderforge.Network.Interfaces.Packets;
using Elderforge.Network.Interfaces.Services;
using Elderforge.Network.Types;
using Serilog;

namespace Elderforge.Network.Services;

public class NetworkMessageFactory : INetworkMessageFactory
{
    private readonly ILogger _logger = Log.ForContext<NetworkMessageFactory>();

    private ReadOnlyDictionary<NetworkMessageType, Type> _messageTypes = new(new Dictionary<NetworkMessageType, Type>());

    private INetworkMessageDecoder _decoder;

    private INetworkMessageEncoder _encoder;

    public void RegisterEncoder(INetworkMessageEncoder encoder)
    {
        _encoder = encoder;
        _logger.Debug("Registered encoder {encoder}", encoder.GetType().Name);
    }

    public void RegisterDecoder(INetworkMessageDecoder decoder)
    {
        _decoder = decoder;
        _logger.Debug("Registered decoder {decoder}", decoder.GetType().Name);
    }

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

    public async Task<INetworkPacket> SerializeAsync<T>(T message) where T : class, INetworkMessage
    {
        if (_encoder == null)
        {
            _logger.Error("No encoder registered");
            throw new InvalidOperationException("No message encoder registered");
        }


        var messageType = _messageTypes.FirstOrDefault(x => x.Value == message.GetType()).Key;

        return _encoder.Encode(message, messageType);
    }

    public async Task<INetworkMessage> ParseAsync(INetworkPacket packet)
    {
        if (_decoder == null)
        {
            _logger.Error("No decoder registered");
            throw new InvalidOperationException("No message decoder registered");
        }

        if (!_messageTypes.TryGetValue(packet.MessageType, out var type))
        {
            _logger.Error("No message type registered for {messageType}", packet.MessageType);
            throw new InvalidOperationException("No message type registered");
        }

        var message = _decoder.Decode(packet, type);

        return message;
    }
}
