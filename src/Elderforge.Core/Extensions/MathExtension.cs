using System.Numerics;

namespace Elderforge.Core.Extensions;

public static class MathExtension
{
    public static float SqrMagnitude(this Vector3 vector)
    {
        return vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z;
    }


    public static float SqrMagnitude(this Vector3 pointA, Vector3 pointB)
    {
        var difference = pointA - pointB;
        return SqrMagnitude(difference);
    }

    public static int Mod(int a, int b)
    {
        return (a % b + b) % b;
    }
}
