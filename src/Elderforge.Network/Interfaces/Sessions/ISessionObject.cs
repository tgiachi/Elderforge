using LiteNetLib;
using LiteNetLib.Utils;

namespace Elderforge.Network.Interfaces.Sessions;

public interface ISessionObject
{
    NetPeer Peer { get; }

    NetDataWriter Writer { get; }
}
