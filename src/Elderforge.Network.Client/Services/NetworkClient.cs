using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Elderforge.Network.Client.Interfaces;
using Elderforge.Network.Data.Internal;
using Elderforge.Network.Encoders;
using Elderforge.Network.Interfaces.Messages;
using Elderforge.Network.Interfaces.Services;
using Elderforge.Network.Packets.Base;
using Elderforge.Network.Services;
using LiteNetLib;
using LiteNetLib.Utils;
using Serilog;

namespace Elderforge.Network.Client.Services;

public class NetworkClient : INetworkClient
{
    public event INetworkClient.MessageReceivedEventHandler? MessageReceived;
    public event EventHandler? Connected;

    public bool IsConnected { get; private set; }


    private readonly ILogger _logger = Log.Logger.ForContext<NetworkClient>();
    private readonly EventBasedNetListener _clientListener = new();

    private readonly Subject<INetworkMessage> _messageSubject = new();

    private readonly NetManager _netManager;

    private bool _connected;


    private readonly INetworkMessageFactory _networkMessageFactory;
    private readonly IMessageTypesService _messageTypesService;

    private readonly NetPacketProcessor _netPacketProcessor = new();

    private readonly NetDataWriter writer = new();

    public NetworkClient(List<MessageTypeObject> messageTypes)
    {
        _netManager = new NetManager(_clientListener);
        _messageTypesService = new MessageTypesService(messageTypes);
        _networkMessageFactory = new NetworkMessageFactory(
            _messageTypesService,
            new ProtobufDecoder(),
            new ProtobufEncoder()
        );

        _netPacketProcessor.SubscribeReusable<NetworkPacket, NetPeer>(OnReceivePacket);

        _clientListener.NetworkReceiveEvent += OnMessageReceived;
    }

    private void OnReceivePacket(NetworkPacket packet, NetPeer peer)
    {
        _logger.Debug("Received packet from server type: {Type}", packet.MessageType);

        if (!_connected)
        {
            _connected = true;
            IsConnected = true;
            Connected?.Invoke(this, EventArgs.Empty);
        }

        var message = _networkMessageFactory.ParseAsync(packet).Result;

        _logger.Debug("Parsed message from server type: {Type}", message.GetType().Name);

        MessageReceived?.Invoke(packet.MessageType, message);

        _messageSubject.OnNext(message);
    }

    private void OnMessageReceived(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
    {
        _netPacketProcessor.ReadAllPackets(reader, peer);
    }


    public void Connect(string host, int port)
    {
        _netManager.Start();
        _netManager.Connect(host, port, string.Empty);

        IsConnected = true;
    }

    public async Task SendMessageAsync<T>(T message) where T : class, INetworkMessage
    {
        if (!IsConnected)
        {
            _logger.Warning("Dropping message {messageType} as client is not connected", message.GetType().Name);
            return;
        }


        var packet = (NetworkPacket)(await _networkMessageFactory.SerializeAsync(message));

        _logger.Debug(">> Sending message {messageType}", message.GetType().Name);

        writer.Reset();

        _netPacketProcessor.Write(writer, packet);

        _netManager.FirstPeer.Send(writer, DeliveryMethod.ReliableOrdered);
    }

    public IObservable<T> SubscribeToMessage<T>() where T : class, INetworkMessage
    {
        return _messageSubject.OfType<T>();
    }


    public void PoolEvents()
    {
        if (_netManager.IsRunning)
        {
            _netManager.PollEvents();
        }
    }
}
