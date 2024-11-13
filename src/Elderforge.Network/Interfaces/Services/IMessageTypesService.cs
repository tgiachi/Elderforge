using System;
using Elderforge.Network.Types;

namespace Elderforge.Network.Interfaces.Services;

public interface IMessageTypesService
{
    Type GetMessageType(NetworkMessageType messageType);
    NetworkMessageType GetMessageType(Type type);

    NetworkMessageType GetMessageType<T>() where T : class;

    void RegisterMessageType(NetworkMessageType messageType, Type type);
}
