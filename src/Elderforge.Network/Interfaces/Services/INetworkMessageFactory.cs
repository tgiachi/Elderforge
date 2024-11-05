using System;
using Elderforge.Network.Interfaces.Encoders;
using Elderforge.Network.Types;

namespace Elderforge.Network.Interfaces.Services;

public interface INetworkMessageFactory
{
    void RegisterEncoder(INetworkMessageEncoder encoder);

    void RegisterDecoder(INetworkMessageDecoder decoder);

    void RegisterMessageType(NetworkMessageType messageType, Type type);
}
