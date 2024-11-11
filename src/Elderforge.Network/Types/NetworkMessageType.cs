namespace Elderforge.Network.Types;

public enum NetworkMessageType : byte
{
    Version,
    Ping,
    Chat,
    Motd,
}
