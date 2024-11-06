using Elderforge.Network.Data.Internal;
using Elderforge.Network.Encoders;
using Elderforge.Network.Interfaces.Services;
using Elderforge.Network.Packets;
using Elderforge.Network.Services;
using Elderforge.Network.Types;

namespace Elderforge.Tests;

public class NetworkPacketsTests : IDisposable
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
        var networkMessageFactory = new NetworkMessageFactory(
            _messageTypesService,
            new ProtobufDecoder(),
            new ProtobufEncoder()
        );


        var pingMessage = new PingMessage(DateTime.Now);

        var packet = await networkMessageFactory.SerializeAsync(pingMessage);

        Assert.NotNull(packet.Payload);
    }

    [Fact]
    public async Task TestPacketParse()
    {
        var networkMessageFactory = new NetworkMessageFactory(
            _messageTypesService,
            new ProtobufDecoder(),
            new ProtobufEncoder()
        );

        var pingMessage = new PingMessage(DateTime.Now);

        var packet = await networkMessageFactory.SerializeAsync(pingMessage);

        Assert.NotNull(packet.Payload);

        var parsedMessage = await networkMessageFactory.ParseAsync(packet) as PingMessage;

        Assert.NotNull(parsedMessage);

        Assert.Equal(pingMessage.Timestamp, parsedMessage.Timestamp);
    }


    [Fact]
    public async Task TestMessageDispatcherWithDispatchMessage()
    {
        var factory = new NetworkMessageFactory(_messageTypesService, new ProtobufDecoder(), new ProtobufEncoder());
        var messageDispatcherService = new MessageDispatcherService(
            _messageTypesService,
            factory
        );

        messageDispatcherService.RegisterMessageListener<PingMessage>(
            async (s, message) =>
            {
                Assert.NotNull(message);
                return Array.Empty<SessionNetworkMessage>();
            }
        );

        var pingMessage = new PingMessage(DateTime.Now);

        messageDispatcherService.DispatchMessage(string.Empty, pingMessage);
    }

    [Fact]
    public async Task TestMessageDispatcherWithChannelWriter()
    {
        var factory = new NetworkMessageFactory(_messageTypesService, new ProtobufDecoder(), new ProtobufEncoder());

        var messageDispatcherService = new MessageDispatcherService(_messageTypesService, factory);

        messageDispatcherService.RegisterMessageListener<PingMessage>(
            async (s, message) =>
            {
                Assert.NotNull(message);
                return Array.Empty<SessionNetworkMessage>();
            }
        );

        var pingMessage = new PingMessage(DateTime.Now);

        var packet = await factory.SerializeAsync(pingMessage);


        messageDispatcherService.GetOutgoingMessagesChannel().TryWrite(new SessionNetworkPacket(string.Empty, packet));
    }

    public void Dispose()
    {
    }
}
