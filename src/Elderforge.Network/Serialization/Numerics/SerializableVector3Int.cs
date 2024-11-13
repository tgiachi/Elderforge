using Elderforge.Core.Numerics;
using ProtoBuf;

namespace Elderforge.Network.Serialization.Numerics;

[ProtoContract]
public class SerializableVector3Int
{
    [ProtoMember(1)]
    public int X { get; set; }

    [ProtoMember(2)]
    public int Y { get; set; }

    [ProtoMember(3)]
    public int Z { get; set; }


    public SerializableVector3Int() { }

    public SerializableVector3Int(Vector3Int vector)
    {
        X = vector.X;
        Y = vector.Y;
        Z = vector.Z;
    }

    public Vector3Int ToVector3Int()
    {
        return new Vector3Int(X, Y, Z);
    }
}
