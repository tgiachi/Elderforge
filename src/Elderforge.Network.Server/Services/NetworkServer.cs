using Elderforge.Network.Data.Session;
using Elderforge.Network.Interfaces.Services;
using Elderforge.Network.Server.Data;
using LiteNetLib;
using Serilog;

namespace Elderforge.Network.Server.Services;

public class NetworkServer<TSession> : INetworkServer where TSession : class
{
    private readonly ILogger _logger = Log.ForContext<INetworkServer>();

    private readonly IMessageDispatcherService _messageDispatcherService;

    private readonly NetworkServerConfig _config;

    private readonly IMessageParserWriterService _messageParserWriterService;

    private readonly INetworkSessionService<TSession> _networkSessionService;

    private readonly EventBasedNetListener _serverListener = new();

    private readonly NetManager _netServer;

    public NetworkServer(
        IMessageDispatcherService messageDispatcherService, IMessageParserWriterService messageParserWriterService,
        INetworkSessionService<TSession> networkSessionService, NetworkServerConfig config
    )
    {
        _config = config;
        _messageDispatcherService = messageDispatcherService;
        _messageParserWriterService = messageParserWriterService;
        _networkSessionService = networkSessionService;


        _serverListener.ConnectionRequestEvent += OnConnectionRequested;
        _serverListener.PeerDisconnectedEvent += OnPeerDisconnection;
        _serverListener.NetworkReceiveEvent += OnNetworkEvent;

        _netServer = new NetManager(_serverListener);
    }

    private void OnNetworkEvent(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
    {
        _messageParserWriterService.ReadPackets(reader, peer);
    }

    private void OnPeerDisconnection(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        _logger.Information("Peer {peerId} disconnected", peer.Id);

        _networkSessionService.RemoveSession(peer.Id.ToString());
    }

    private void OnConnectionRequested(ConnectionRequest request)
    {
        _logger.Information("Connection request from {endPoint}", request.RemoteEndPoint);

        var peer = request.Accept();

        _networkSessionService.AddSession(peer.Id.ToString(), new SessionObject<TSession>(peer, default));
    }

    public Task StartAsync()
    {
        _logger.Information("Starting server on port: {Port}", _config.Port);

        _netServer.Start(_config.Port);

        return Task.CompletedTask;
    }

    public Task StopAsync()
    {
        _logger.Information("Stopping server");

        _netServer.Stop();

        return Task.CompletedTask;
    }
}
