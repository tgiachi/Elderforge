using System;

namespace Elderforge.Network.Types;

[Flags]
public enum NetworkPacketType : byte
{
    None,
    Compressed
}
