using System.Threading.Channels;
using System.Threading.Tasks;
using Elderforge.Network.Data.Internal;
using Elderforge.Network.Interfaces.Messages;
using Elderforge.Network.Interfaces.Services;
using Elderforge.Network.Packets.Base;
using LiteNetLib;
using LiteNetLib.Utils;
using Serilog;

namespace Elderforge.Network.Services;

public class MessageParserWriterService : IMessageParserWriterService
{
    public ChannelWriter<SessionNetworkMessage> SessionMessagesWriter => _sessionMessages.Writer;

    public ChannelReader<SessionNetworkMessage> SessionMessagesReader => _sessionMessages.Reader;

    private readonly ILogger _logger = Log.ForContext<MessageParserWriterService>();

    private readonly NetPacketProcessor _netPacketProcessor = new();

    private readonly Channel<SessionNetworkMessage> _sessionMessages = Channel.CreateUnbounded<SessionNetworkMessage>();


    private readonly INetworkMessageFactory _networkMessageFactory;


    public MessageParserWriterService(INetworkMessageFactory networkMessageFactory)
    {
        _networkMessageFactory = networkMessageFactory;
        _netPacketProcessor.SubscribeReusable<NetworkPacket, NetPeer>(OnReceivePacket);
    }


    private async void OnReceivePacket(NetworkPacket packet, NetPeer peer)
    {
        _logger.Debug("Received packet from {peerId} type: {Type}", peer.Id, packet.MessageType);

        var message = await _networkMessageFactory.ParseAsync(packet);

        _sessionMessages.Writer.TryWrite(new SessionNetworkMessage(peer.Id.ToString(), message));
    }

    public void ReadPackets(NetDataReader reader, NetPeer peer)
    {
        _netPacketProcessor.ReadAllPackets(reader, peer);
    }

    public async Task WriteMessageAsync(NetPeer peer, NetDataWriter writer, INetworkMessage message)
    {
        writer.Reset();

        var packet = await _networkMessageFactory.SerializeAsync(message);

        _logger.Debug("Writing message to {peerId} type: {Type}", peer.Id, packet.MessageType);

        _netPacketProcessor.Write(writer, (NetworkPacket)packet);

        peer.Send(writer, DeliveryMethod.ReliableOrdered);
    }
}
