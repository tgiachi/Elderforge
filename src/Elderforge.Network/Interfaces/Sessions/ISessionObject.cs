using System.Collections.Generic;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Elderforge.Network.Interfaces.Sessions;

public interface ISessionObject
{
    NetPeer Peer { get; }

    NetDataWriter Writer { get; }

    Dictionary<string, object> Data { get; }

    TDataObject GetDataObject<TDataObject>(string key, bool throwIfNowExist) where TDataObject : class;
}
