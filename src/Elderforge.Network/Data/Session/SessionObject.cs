using System;
using System.Collections.Generic;
using Elderforge.Network.Interfaces.Sessions;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Elderforge.Network.Data.Session;

public class SessionObject : ISessionObject
{
    public string Id { get; }
    public NetPeer Peer { get; }
    public Dictionary<string, object> Data { get; }
    public NetDataWriter Writer { get; }

    public bool IsLoggedIn { get; set; }

    public DateTime LastActive { get; set; }

    public SessionObject(NetPeer peer)
    {
        Peer = peer;
        Data = new Dictionary<string, object>();
        Writer = new NetDataWriter();
        LastActive = DateTime.UtcNow;
        Id = peer.Id.ToString();
    }

    public TDataObject GetDataObject<TDataObject>(string key, bool throwIfNowExist = true)
    {
        if (Data.TryGetValue(key, out var value))
        {
            return value is TDataObject ? (TDataObject)value : default;
        }

        if (throwIfNowExist)
        {
            throw new KeyNotFoundException($"Key {key} not found in session data");
        }

        return default;
    }

    public override string ToString()
    {
        return $"Peer: {Peer.Id}, Data: {Data}";
    }
}
