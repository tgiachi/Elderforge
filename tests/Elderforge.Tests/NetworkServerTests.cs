using Elderforge.Network.Encoders;
using Elderforge.Network.Interfaces.Services;
using Elderforge.Network.Packets;
using Elderforge.Network.Packets.Base;
using Elderforge.Network.Server.Data;
using Elderforge.Network.Server.Services;
using Elderforge.Network.Services;
using Elderforge.Network.Types;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Elderforge.Tests;

public class NetworkServerTests
{
    private readonly IMessageTypesService _messageTypesService;
    private readonly INetworkMessageFactory _networkMessageFactory;
    private readonly IMessageDispatcherService _messageDispatcherService;
    private readonly IMessageParserWriterService _messageParserWriterService;
    private readonly INetworkSessionService<string> _networkSessionService;

    private readonly NetPacketProcessor _netPacketProcessor = new NetPacketProcessor();

    public NetworkServerTests()
    {
        _messageTypesService = new MessageTypesService();
        _messageTypesService.RegisterMessageType(NetworkMessageType.Ping, typeof(PingMessage));
        _networkMessageFactory = new NetworkMessageFactory(
            _messageTypesService,
            new ProtobufDecoder(),
            new ProtobufEncoder()
        );

        _messageDispatcherService = new MessageDispatcherService(_messageTypesService, _networkMessageFactory);
        _messageParserWriterService = new MessageParserWriterService(_networkMessageFactory);
        _networkSessionService = new NetworkSessionService<string>();
    }


    [Fact]
    public async Task TestNetworkServer()
    {
        var networkServer = new NetworkServer<string>(
            _messageDispatcherService,
            _messageParserWriterService,
            _networkSessionService,
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
    public async Task TestNetworkServerWithClient()
    {
        var clientListener = new EventBasedNetListener();

        var networkServer = new NetworkServer<string>(
            _messageDispatcherService,
            _messageParserWriterService,
            _networkSessionService,
            new NetworkServerConfig
            {
                Port = 5000
            }
        );

        networkServer.StartAsync();

        await Task.Delay(1000);

        var _client = new NetManager(clientListener);

        var message = new PingMessage();
        var packet = await _networkMessageFactory.SerializeAsync(message);
        var messageWriter = new NetDataWriter();

        _netPacketProcessor.Write(messageWriter, (NetworkPacket)packet);


        _client.Start();
        _client.Connect("localhost", new NetworkServerConfig().Port, string.Empty);

        var counter = 0;

        while (counter < 100)
        {
            _client.FirstPeer?.Send(messageWriter, DeliveryMethod.ReliableOrdered);
            _client.PollEvents();
            await Task.Delay(100);
            counter++;
        }
    }
}
