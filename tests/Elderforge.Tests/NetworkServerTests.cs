using Elderforge.Core.Interfaces.Services;
using Elderforge.Core.Services;
using Elderforge.Network.Client.Services;
using Elderforge.Network.Data.Internal;
using Elderforge.Network.Encoders;
using Elderforge.Network.Interfaces.Services;
using Elderforge.Network.Packets;
using Elderforge.Network.Packets.Base;
using Elderforge.Network.Packets.System;
using Elderforge.Network.Server.Data;
using Elderforge.Network.Server.Services;
using Elderforge.Network.Services;
using Elderforge.Network.Types;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Elderforge.Tests;

public class NetworkServerTests
{
    private readonly INetworkMessageFactory _networkMessageFactory;
    private readonly IMessageDispatcherService _messageDispatcherService;
    private readonly IMessageParserWriterService _messageParserWriterService;
    private readonly INetworkSessionService _networkSessionService;
    private readonly IEventBusService _eventBusService = new EventBusService();
    private readonly IMessageChannelService _messageChannelService = new MessageChannelService();

    private readonly NetPacketProcessor _netPacketProcessor = new();

    public NetworkServerTests()
    {
        var messageTypesService = new MessageTypesService();
        messageTypesService.RegisterMessageType(NetworkMessageType.Ping, typeof(PingMessage));
        _networkMessageFactory = new NetworkMessageFactory(
            messageTypesService,
            new ProtobufDecoder(),
            new ProtobufEncoder()
        );

        _messageDispatcherService = new MessageDispatcherService(
            messageTypesService,
            _networkMessageFactory,
            _messageChannelService
        );
        _messageParserWriterService = new MessageParserWriterService(_networkMessageFactory, _messageChannelService);
        _networkSessionService = new NetworkSessionService();
    }


    [Fact]
    public async Task TestNetworkServer()
    {
        var networkServer = new NetworkServer(
            _messageDispatcherService,
            _messageParserWriterService,
            _networkSessionService,
            _eventBusService,
            _messageChannelService,
            _networkMessageFactory,
            new NetworkServerConfig
            {
                Port = 5000
            }
        );

        networkServer.StartAsync();

        await Task.Delay(1000);

        Assert.True(networkServer.IsRunning);

        networkServer.StopAsync();
    }

    [Fact]
    public async Task TestNetworkServerWithRawClient()
    {
        const int maxMessages = 100;
        var amount = 0;

        var clientListener = new EventBasedNetListener();

        var networkServer = new NetworkServer(
            _messageDispatcherService,
            _messageParserWriterService,
            _networkSessionService,
            _eventBusService,
            _messageChannelService,
            _networkMessageFactory,
            new NetworkServerConfig
            {
                Port = 5000
            }
        );


        networkServer.RegisterMessageListener(
            async (string session, PingMessage message) =>
            {
                Assert.NotNull(message);
                Assert.NotNull(session);

                Interlocked.Add(ref amount, 1);

                return ArraySegment<SessionNetworkMessage>.Empty;
            }
        );

        networkServer.StartAsync();

        var client = new NetManager(clientListener);

        var message = new PingMessage();
        var packet = await _networkMessageFactory.SerializeAsync(message);
        var messageWriter = new NetDataWriter();

        _netPacketProcessor.Write(messageWriter, (NetworkPacket)packet);

        client.Start();
        client.Connect("localhost", new NetworkServerConfig().Port, string.Empty);

        var counter = 0;

        while (counter < maxMessages)
        {
            client.FirstPeer?.Send(messageWriter, DeliveryMethod.ReliableUnordered);
            client.PollEvents();
            await Task.Delay(10);
            amount += 1;
            counter++;
        }

        Assert.True(amount >= 0);

        client.Stop();

        networkServer.StopAsync();
    }

    [Fact]
    public async Task TestNetworkServerWithClient()
    {
        const int maxMessages = 100;
        var amount = 0;


        var networkClient = new NetworkClient(
            new List<MessageTypeObject>
            {
                new MessageTypeObject(NetworkMessageType.Ping, typeof(PingMessage))
            }
        );


        networkClient.SubscribeToMessage<PingMessage>()
            .Subscribe(
                message =>
                {
                    Assert.NotNull(message);
                    Interlocked.Add(ref amount, 1);
                }
            );


        var networkServer = new NetworkServer(
            _messageDispatcherService,
            _messageParserWriterService,
            _networkSessionService,
            _eventBusService,
            _messageChannelService,
            _networkMessageFactory,
            new NetworkServerConfig
            {
                Port = 5000
            }
        );


        networkServer.RegisterMessageListener(
            async (string session, PingMessage message) =>
            {
                Assert.NotNull(message);
                Assert.NotNull(session);

                Interlocked.Add(ref amount, 1);

                return new[]
                {
                    new SessionNetworkMessage(session, message), new SessionNetworkMessage(session, message),
                    new SessionNetworkMessage(session, message), new SessionNetworkMessage(session, message)
                };
            }
        );

        networkServer.StartAsync();

        networkClient.Connect("localhost", 5000);

        networkClient.SendMessageAsync(new PingMessage());

        var count = 0;

        while (count < maxMessages)
        {
            networkClient.PoolEvents();
            await Task.Delay(10);
            count++;
        }
    }
}
