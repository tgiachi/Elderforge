using Elderforge.Network.Encoders;
using Elderforge.Network.Packets;
using Elderforge.Network.Services;
using Elderforge.Network.Types;

namespace Elderforge.Tests;

public class NetworkPacketsTests
{
    [Fact]
    public async Task TestPacketBuild()
    {
        var networkMessageFactory = new NetworkMessageFactory();

        networkMessageFactory.RegisterEncoder(new ProtobufEncoder());
        networkMessageFactory.RegisterDecoder(new ProtobufDecoder());

        networkMessageFactory.RegisterMessageType(NetworkMessageType.Ping, typeof(PingMessage));


        var pingMessage = new PingMessage(DateTime.Now);

        var packet = await networkMessageFactory.SerializeAsync(pingMessage);

        Assert.NotNull(packet.Payload);
    }

    [Fact]
    public async Task TestPacketParse()
    {
        var networkMessageFactory = new NetworkMessageFactory();

        networkMessageFactory.RegisterEncoder(new ProtobufEncoder());
        networkMessageFactory.RegisterDecoder(new ProtobufDecoder());

        networkMessageFactory.RegisterMessageType(NetworkMessageType.Ping, typeof(PingMessage));

        var pingMessage = new PingMessage(DateTime.Now);

        var packet = await networkMessageFactory.SerializeAsync(pingMessage);

        Assert.NotNull(packet.Payload);

        var parsedMessage = await networkMessageFactory.ParseAsync(packet) as PingMessage;

        Assert.NotNull(parsedMessage);

        Assert.Equal(pingMessage.Timestamp, parsedMessage.Timestamp);
    }
}
