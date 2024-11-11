using Elderforge.Shared.Blocks;
using ProtoBuf;

namespace Elderforge.Core.Server.Serialization.Map;


[ProtoContract]
public class BlockSerializable
{
    [ProtoMember(1)]
    public BlockEntity BlockEntity { get; set; }
}
