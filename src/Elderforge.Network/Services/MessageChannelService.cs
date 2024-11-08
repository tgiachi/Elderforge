using System.Threading.Channels;
using Elderforge.Network.Data.Internal;
using Elderforge.Network.Interfaces.Services;

namespace Elderforge.Network.Services;

public class MessageChannelService : IMessageChannelService
{
    private readonly Channel<SessionNetworkPacket> _incomingChannel;
    private readonly Channel<SessionNetworkMessage> _outgoingChannel;

    private readonly BoundedChannelOptions _channelsOption;

    public ChannelReader<SessionNetworkPacket> IncomingReaderChannel => _incomingChannel;
    public ChannelWriter<SessionNetworkPacket> IncomingWriterChannel => _incomingChannel;
    public ChannelReader<SessionNetworkMessage> OutgoingReaderChannel => _outgoingChannel;
    public ChannelWriter<SessionNetworkMessage> OutgoingWriterChannel => _outgoingChannel;

    public MessageChannelService()
    {
        _channelsOption = new BoundedChannelOptions(1024)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = false,
            SingleWriter = false
        };
        _incomingChannel = Channel.CreateBounded<SessionNetworkPacket>(_channelsOption);
        _outgoingChannel = Channel.CreateBounded<SessionNetworkMessage>(_channelsOption);
    }

    public void Dispose()
    {
        _incomingChannel.Writer.Complete();
        _outgoingChannel.Writer.Complete();
    }
}
