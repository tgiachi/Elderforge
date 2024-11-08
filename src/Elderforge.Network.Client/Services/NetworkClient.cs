using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
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
    private readonly ILogger _logger = Log.Logger.ForContext<NetworkClient>();
    private readonly EventBasedNetListener _clientListener = new();

    private readonly Subject<INetworkMessage> _messageSubject = new();

    private readonly NetManager _netManager;
    private readonly string _host;
    private readonly int _port;

    private readonly INetworkMessageFactory _networkMessageFactory;
    private readonly IMessageTypesService _messageTypesService;

    private readonly NetPacketProcessor _netPacketProcessor = new();

    private readonly NetDataWriter writer = new();

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

        _netPacketProcessor.SubscribeReusable<NetworkPacket, NetPeer>(OnReceivePacket);

        _clientListener.NetworkReceiveEvent += OnMessageReceived;
    }

    private void OnReceivePacket(NetworkPacket packet, NetPeer peer)
    {
        _logger.Debug("Received packet from server type: {Type}", packet.MessageType);

        var message = _networkMessageFactory.ParseAsync(packet).Result;

        _logger.Debug("Parsed message from server type: {Type}", message.GetType().Name);

        _messageSubject.OnNext(message);
    }

    private void OnMessageReceived(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
    {
        _netPacketProcessor.ReadAllPackets(reader, peer);
    }


    public void Connect()
    {
        _netManager.Start();
        _netManager.Connect(_host, _port, string.Empty);
    }

    public void SendMessage<T>(T message) where T : class, INetworkMessage
    {
        var packet = (NetworkPacket)_networkMessageFactory.SerializeAsync(message).Result;

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
