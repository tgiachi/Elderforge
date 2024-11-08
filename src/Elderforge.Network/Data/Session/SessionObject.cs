using System.Collections.Generic;
using Elderforge.Network.Interfaces.Sessions;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Elderforge.Network.Data.Session;

public class SessionObject : ISessionObject
{
    public NetPeer Peer { get; }
    public Dictionary<string, object> Data { get; }

    public NetDataWriter Writer { get; }

    public SessionObject(NetPeer peer)
    {
        Peer = peer;
        Data = new Dictionary<string, object>();
        Writer = new NetDataWriter();
    }

    public TDataObject GetDataObject<TDataObject>(string key, bool throwIfNowExist) where TDataObject : class
    {
        if (Data.TryGetValue(key, out var value))
        {
            return value as TDataObject;
        }

        if (throwIfNowExist)
        {
            throw new KeyNotFoundException($"Key {key} not found in session data");
        }

        return null;
    }

    public override string ToString()
    {
        return $"Peer: {Peer.Id}, Data: {Data}";
    }
}
