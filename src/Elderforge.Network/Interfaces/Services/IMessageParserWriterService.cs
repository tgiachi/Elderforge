using System;
using System.Threading.Channels;
using System.Threading.Tasks;
using Elderforge.Network.Data.Internal;
using Elderforge.Network.Interfaces.Messages;
using Elderforge.Network.Interfaces.Packets;
using Elderforge.Network.Packets.Base;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Elderforge.Network.Interfaces.Services;

public interface IMessageParserWriterService
{
    ChannelWriter<SessionNetworkMessage> SessionMessagesWriter { get; }

    ChannelReader<SessionNetworkMessage> SessionMessagesReader { get; }

    void ReadPackets(NetDataReader reader, NetPeer peer);

    Task WriteMessageAsync(NetPeer peer, NetDataWriter writer, NetworkPacket message);
}
