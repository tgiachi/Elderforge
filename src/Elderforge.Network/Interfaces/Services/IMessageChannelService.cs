using System;
using System.Threading.Channels;
using Elderforge.Network.Data.Internal;

namespace Elderforge.Network.Interfaces.Services;

public interface IMessageChannelService : IDisposable
{
    ChannelReader<SessionNetworkPacket> IncomingReaderChannel { get; }
    ChannelWriter<SessionNetworkPacket> IncomingWriterChannel { get; }
    ChannelReader<SessionNetworkMessage> OutgoingReaderChannel { get; }
    ChannelWriter<SessionNetworkMessage> OutgoingWriterChannel { get; }
}
