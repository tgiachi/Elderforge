using System.Threading.Tasks;
using Elderforge.Network.Data.Internal;
using Elderforge.Network.Interfaces.Services;
using Elderforge.Network.Packets.Base;
using LiteNetLib;
using LiteNetLib.Utils;
using Serilog;

namespace Elderforge.Network.Services;

public class MessageParserWriterService : IMessageParserWriterService
{
    private readonly ILogger _logger = Log.ForContext<MessageParserWriterService>();

    private readonly NetPacketProcessor _netPacketProcessor = new();

    private readonly INetworkMessageFactory _networkMessageFactory;

    private readonly IMessageChannelService _messageChannelService;

    public MessageParserWriterService(
        INetworkMessageFactory networkMessageFactory, IMessageChannelService messageChannelService
    )
    {
        _networkMessageFactory = networkMessageFactory;
        _messageChannelService = messageChannelService;
        _netPacketProcessor.SubscribeReusable<NetworkPacket, NetPeer>(OnReceivePacket);
    }

    private async void OnReceivePacket(NetworkPacket packet, NetPeer peer)
    {
        _logger.Debug("Received packet from {peerId} type: {Type}", peer.Id, packet.MessageType);
        _messageChannelService.IncomingWriterChannel.TryWrite(new SessionNetworkPacket(peer.Id.ToString(), packet));
    }

    public void ReadPackets(NetDataReader reader, NetPeer peer)
    {
        _netPacketProcessor.ReadAllPackets(reader, peer);
    }

    public async Task WriteMessageAsync(NetPeer peer, NetDataWriter writer, NetworkPacket message)
    {
        writer.Reset();

        _logger.Debug("Writing message to {peerId} type: {Type}", peer.Id, message.MessageType);

        _netPacketProcessor.Write(writer, message);

        peer.Send(writer, DeliveryMethod.ReliableOrdered);
    }
}
