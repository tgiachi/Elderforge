using Elderforge.Network.Encoders;
using Elderforge.Network.Interfaces.Services;
using Elderforge.Network.Packets;
using Elderforge.Network.Services;
using Elderforge.Network.Types;

namespace Elderforge.Tests;

public class NetworkPacketsTests
{
    private readonly IMessageTypesService _messageTypesService;


    public NetworkPacketsTests()
    {
        _messageTypesService = new MessageTypesService();
        _messageTypesService.RegisterMessageType(NetworkMessageType.Ping, typeof(PingMessage));
    }

    [Fact]
    public async Task TestPacketBuild()
    {
        var networkMessageFactory = new NetworkMessageFactory(_messageTypesService);

        networkMessageFactory.RegisterEncoder(new ProtobufEncoder());
        networkMessageFactory.RegisterDecoder(new ProtobufDecoder());


        var pingMessage = new PingMessage(DateTime.Now);

        var packet = await networkMessageFactory.SerializeAsync(pingMessage);

        Assert.NotNull(packet.Payload);
    }

    [Fact]
    public async Task TestPacketParse()
    {
        var networkMessageFactory = new NetworkMessageFactory(_messageTypesService);

        networkMessageFactory.RegisterEncoder(new ProtobufEncoder());
        networkMessageFactory.RegisterDecoder(new ProtobufDecoder());


        var pingMessage = new PingMessage(DateTime.Now);

        var packet = await networkMessageFactory.SerializeAsync(pingMessage);

        Assert.NotNull(packet.Payload);

        var parsedMessage = await networkMessageFactory.ParseAsync(packet) as PingMessage;

        Assert.NotNull(parsedMessage);

        Assert.Equal(pingMessage.Timestamp, parsedMessage.Timestamp);
    }


    [Fact]
    public async Task TestMessageDispatcher()
    {
    }
}
