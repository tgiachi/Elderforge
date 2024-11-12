namespace Elderforge.Network.Types;

public enum NetworkMessageType : byte
{
    Version,
    Ping,
    Pong,
    Chat,
    Motd,
    MotdRequest,
    ServerReady,
}
