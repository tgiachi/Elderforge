using System.Numerics;
using ProtoBuf;

namespace Elderforge.Network.Serialization.Numerics;


[ProtoContract]
public class SerializableVector3
{
    [ProtoMember(1)]
    public float X { get; set; }

    [ProtoMember(2)]
    public float Y { get; set; }

    [ProtoMember(3)]
    public float Z { get; set; }


    public SerializableVector3() { }

    public SerializableVector3(Vector3 vector)
    {
        X = vector.X;
        Y = vector.Y;
        Z = vector.Z;
    }


    public Vector3 ToVector3()
    {
        return new Vector3(X, Y, Z);
    }
}
