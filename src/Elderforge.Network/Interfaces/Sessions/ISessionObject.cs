using System;
using System.Collections.Generic;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Elderforge.Network.Interfaces.Sessions;

public interface ISessionObject
{

    string Id { get;  }

    NetPeer Peer { get; }

    NetDataWriter Writer { get; }

    Dictionary<string, object> Data { get; }

    DateTime LastActive { get; set; }

    bool IsLoggedIn { get; set; }

    TDataObject GetDataObject<TDataObject>(string key, bool throwIfNowExist = true);

    void SetDataObject<TDataObject>(string key, TDataObject value);
}
