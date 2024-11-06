using System;
using System.Threading.Channels;
using System.Threading.Tasks;
using Elderforge.Network.Data.Internal;
using Elderforge.Network.Interfaces.Messages;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Elderforge.Network.Interfaces.Services;

public interface IMessageParserWriterService
{
    ChannelWriter<SessionNetworkMessage> SessionMessagesWriter { get; }

    ChannelReader<SessionNetworkMessage> SessionMessagesReader { get; }

    void ReadPackets(NetDataReader reader, NetPeer peer);

    Task WriteMessageAsync(NetPeer peer, NetDataWriter writer, INetworkMessage message);
}
