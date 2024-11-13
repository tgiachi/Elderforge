using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Elderforge.Core.Utils;
using Elderforge.Network.Interfaces.Encoders;
using Elderforge.Network.Interfaces.Messages;
using Elderforge.Network.Interfaces.Packets;
using Elderforge.Network.Interfaces.Services;
using Serilog;

namespace Elderforge.Network.Services;

public class NetworkMessageFactory : INetworkMessageFactory
{
    private readonly ILogger _logger = Log.ForContext<NetworkMessageFactory>();

    private readonly IMessageTypesService _messageTypesService;

    private readonly INetworkMessageDecoder _decoder;

    private readonly INetworkMessageEncoder _encoder;

    public NetworkMessageFactory(
        IMessageTypesService messageTypesService, INetworkMessageDecoder decoder, INetworkMessageEncoder encoder
    )
    {
        _messageTypesService = messageTypesService;
        _decoder = decoder;
        _encoder = encoder;
    }


    public async Task<INetworkPacket> SerializeAsync<T>(T message) where T : class
    {
        if (_encoder == null)
        {
            _logger.Error("No encoder registered");
            throw new InvalidOperationException("No message encoder registered");
        }

        var startTime = Stopwatch.GetTimestamp();


        var encodedNetworkPacket = _encoder.Encode(message, _messageTypesService.GetMessageType(message.GetType()));

        var endTime = Stopwatch.GetTimestamp();

        _logger.Debug(
            "Encoding message of type {messageType} took {time}ms",
            message.GetType().Name,
            StopwatchUtils.GetElapsedMilliseconds(startTime, endTime)
        );

        return encodedNetworkPacket;
    }

    public async Task<INetworkMessage> ParseAsync(INetworkPacket packet)
    {
        if (_decoder == null)
        {
            _logger.Error("No decoder registered");
            throw new InvalidOperationException("No message decoder registered");
        }

        var startTime = Stopwatch.GetTimestamp();

        var message = _decoder.Decode(packet, _messageTypesService.GetMessageType(packet.MessageType));

        var endTime = Stopwatch.GetTimestamp();

        _logger.Debug(
            "Decoding message of type {messageType} took {time}ms",
            message.GetType().Name,
            StopwatchUtils.GetElapsedMilliseconds(startTime, endTime)
        );

        return message;
    }
}
