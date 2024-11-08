using System.Threading.Channels;
using Elderforge.Core.Interfaces.Services;
using Elderforge.Core.Server.Events;
using Elderforge.Network.Data.Internal;
using Elderforge.Network.Data.Session;
using Elderforge.Network.Interfaces.Listeners;
using Elderforge.Network.Interfaces.Messages;
using Elderforge.Network.Interfaces.Services;
using Elderforge.Network.Packets.Base;
using Elderforge.Network.Server.Data;
using LiteNetLib;
using Serilog;

namespace Elderforge.Network.Server.Services;

public class NetworkServer<TSession> : INetworkServer where TSession : class
{
    public bool IsRunning { get; private set; }


    private readonly ILogger _logger = Log.ForContext<INetworkServer>();

    private readonly IMessageDispatcherService _messageDispatcherService;

    private readonly IMessageChannelService _messageChannelService;

    private readonly NetworkServerConfig _config;

    private readonly IEventBusService _eventBusService;

    private readonly IMessageParserWriterService _messageParserWriterService;

    private readonly INetworkMessageFactory _networkMessageFactory;

    private readonly INetworkSessionService<TSession> _networkSessionService;

    private readonly CancellationTokenSource _readMessageCancellationTokenSource = new();

    private readonly Task _poolEventTask;

    private readonly EventBasedNetListener _serverListener = new();

    private readonly NetManager _netServer;

    public NetworkServer(
        IMessageDispatcherService messageDispatcherService, IMessageParserWriterService messageParserWriterService,
        INetworkSessionService<TSession> networkSessionService, IEventBusService eventBusService,
        IMessageChannelService messageChannelService, INetworkMessageFactory networkMessageFactory,
        NetworkServerConfig config
    )
    {
        _config = config;
        _networkMessageFactory = networkMessageFactory;
        _messageChannelService = messageChannelService;
        _eventBusService = eventBusService;
        _messageDispatcherService = messageDispatcherService;
        _messageParserWriterService = messageParserWriterService;
        _networkSessionService = networkSessionService;

        _serverListener.ConnectionRequestEvent += OnConnectionRequested;
        _serverListener.PeerDisconnectedEvent += OnPeerDisconnection;
        _serverListener.NetworkReceiveEvent += OnNetworkEvent;

        _netServer = new NetManager(_serverListener);

        _poolEventTask = ServerPoolEvents();
    }

    private async Task ServerPoolEvents()
    {
        _logger.Information("Starting server event loop");

        while (!_readMessageCancellationTokenSource.Token.IsCancellationRequested)
        {
            if (_netServer.IsRunning)
            {
                _netServer.PollEvents();
            }

            await Task.Delay(15);
        }
    }

    // private async Task ReadMessageChannel()
    // {
    //     _logger.Information("Starting read message channel");
    //     while (!_readMessageCancellationTokenSource.Token.IsCancellationRequested)
    //     {
    //         await foreach (var session in _messageParserWriterService.SessionMessagesReader.ReadAllAsync())
    //         {
    //             _messageDispatcherService.DispatchMessage(session.SessionId, session.Packet);
    //         }
    //     }
    // }

    private async Task WriteMessageChannel()
    {
        _logger.Information("Starting write message channel");
        while (!_readMessageCancellationTokenSource.Token.IsCancellationRequested)
        {
            await foreach (var packet in _messageChannelService.OutgoingReaderChannel.ReadAllAsync())
            {
                if (string.IsNullOrEmpty(packet.SessionId))
                {
                    await BroadcastMessageAsync(packet.Packet);
                    continue;
                }

                var session = _networkSessionService.GetSessionObject(packet.SessionId);
                var message = await _networkMessageFactory.SerializeAsync(packet.Packet);

                await _messageParserWriterService.WriteMessageAsync(
                    session.Peer,
                    session.Writer,
                    (NetworkPacket)message
                );
            }
        }
    }

    private void OnNetworkEvent(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
    {
        _messageParserWriterService.ReadPackets(reader, peer);
    }

    private void OnPeerDisconnection(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        _logger.Information("Peer {peerId} disconnected", peer.Id);

        _networkSessionService.RemoveSession(peer.Id.ToString());

        _eventBusService.PublishAsync(new ClientDisconnectedEvent(peer.Id.ToString()));
    }

    private void OnConnectionRequested(ConnectionRequest request)
    {
        _logger.Information("Connection request from {endPoint}", request.RemoteEndPoint);

        var peer = request.Accept();

        _networkSessionService.AddSession(peer.Id.ToString(), new SessionObject<TSession>(peer, default));

        _eventBusService.PublishAsync(new ClientConnectedEvent(peer.Id.ToString()));
    }


    public Task StartAsync()
    {
        _logger.Information("Starting server on port: {Port}", _config.Port);

        _netServer.Start(_config.Port);

        IsRunning = true;

        return Task.CompletedTask;
    }

    public Task StopAsync()
    {
        _logger.Information("Stopping server");

        _netServer.Stop();

        IsRunning = false;

        return Task.CompletedTask;
    }

    public void RegisterMessageListener<TMessage>(INetworkMessageListener<TMessage> listener)
        where TMessage : class, INetworkMessage
    {
        _messageDispatcherService.RegisterMessageListener(listener);
    }

    public void RegisterMessageListener<TMessage>(
        Func<string, TMessage, ValueTask<IEnumerable<SessionNetworkMessage>>> listener
    ) where TMessage : class, INetworkMessage
    {
        _messageDispatcherService.RegisterMessageListener(listener);
    }

    public async ValueTask BroadcastMessageAsync(INetworkMessage message)
    {
        foreach (var sessionId in _networkSessionService.GetSessionIds)
        {
            await SendMessageAsync(new SessionNetworkMessage(sessionId, message));
        }
    }

    public async ValueTask SendMessagesAsync(IEnumerable<SessionNetworkMessage> messages)
    {
        foreach (var messageToSend in messages)
        {
            _messageChannelService.OutgoingWriterChannel.WriteAsync(messageToSend);
        }
    }

    public ValueTask SendMessageAsync(SessionNetworkMessage messages)
    {
        return SendMessagesAsync(new List<SessionNetworkMessage> { messages });
    }


    public void Dispose()
    {
        _messageDispatcherService.Dispose();
        _readMessageCancellationTokenSource.Dispose();
        _poolEventTask.Dispose();
    }
}
