using System.Collections.Generic;
using Elderforge.Network.Client.Interfaces;
using Elderforge.Network.Data.Internal;
using Elderforge.Network.Encoders;
using Elderforge.Network.Interfaces.Services;
using Elderforge.Network.Services;
using LiteNetLib;
using Serilog;

namespace Elderforge.Network.Client.Services;

public class NetworkClient : INetworkClient
{
    private readonly ILogger _logger = Log.Logger.ForContext<NetworkClient>();
    private readonly EventBasedNetListener _clientListener = new();

    private readonly NetManager _netManager;
    private readonly string _host;
    private readonly int _port;

    private readonly INetworkMessageFactory _networkMessageFactory;
    private readonly IMessageTypesService _messageTypesService;


    public NetworkClient(string host, int port, List<MessageTypeObject> messageTypes)
    {
        _host = host;
        _port = port;
        _netManager = new NetManager(_clientListener);
        _messageTypesService = new MessageTypesService(messageTypes);
        _networkMessageFactory = new NetworkMessageFactory(
            _messageTypesService,
            new ProtobufDecoder(),
            new ProtobufEncoder()
        );

        _clientListener.NetworkReceiveEvent += OnMessageReceived;
    }

    private void OnMessageReceived(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
    {

    }


    public void Connect()
    {
        _netManager.Start();
        _netManager.Connect(_host, _port, string.Empty);
    }


    public void PoolEvents()
    {
        if (_netManager.IsRunning)
        {
            _netManager.PollEvents();
        }
    }
}
