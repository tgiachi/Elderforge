using Elderforge.Network.Interfaces.Sessions;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Elderforge.Network.Data.Session;

public class SessionObject<TData> : ISessionObject where TData : class
{
    public NetPeer Peer { get; }

    public TData Data { get; set; }

    public NetDataWriter Writer { get; }

    public SessionObject(NetPeer peer, TData data)
    {
        Peer = peer;
        Data = data;
        Writer = new NetDataWriter();
    }

    public override string ToString()
    {
        return $"Peer: {Peer.Id}, Data: {Data}";
    }
}
