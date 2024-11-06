using System;
using System.Threading.Tasks;
using Elderforge.Network.Interfaces.Encoders;
using Elderforge.Network.Interfaces.Messages;
using Elderforge.Network.Interfaces.Packets;
using Elderforge.Network.Types;

namespace Elderforge.Network.Interfaces.Services;

public interface INetworkMessageFactory
{
    void RegisterEncoder(INetworkMessageEncoder encoder);

    void RegisterDecoder(INetworkMessageDecoder decoder);


    Task<INetworkPacket> SerializeAsync<T>(T message) where T : class, INetworkMessage;

    Task<INetworkMessage> ParseAsync(INetworkPacket packet);
}
