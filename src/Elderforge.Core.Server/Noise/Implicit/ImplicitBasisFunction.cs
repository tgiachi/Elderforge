﻿using Elderforge.Core.Server.Noise.Enums;

namespace Elderforge.Core.Server.Noise.Implicit;

public sealed class ImplicitBasisFunction : ImplicitModuleBase
{
    private readonly double[] offset = new double[4];

    private readonly double[,] rotationMatrix = new double[3, 3];
    private readonly double[] scale = new double[4];

    private BasisType basisType;

    private double cos2D;

    private InterpolationType interpolationType;

    private InterpolationDelegate interpolator;

    private Noise2DDelegate noise2D;

    private Noise3DDelegate noise3D;

    private Noise4DDelegate noise4D;

    private Noise6DDelegate noise6D;

    private int seed;

    private double sin2D;

    public ImplicitBasisFunction(
        BasisType basisType = BasisType.Gradient, InterpolationType interpolationType = InterpolationType.Quintic
    )
    {
        BasisType = basisType;
        InterpolationType = interpolationType;
        Seed = (int)DateTime.Now.Ticks;
    }

    public override int Seed
    {
        get => seed;
        set
        {
            seed = value;
            var random = new Random(value);

            var ax = random.NextDouble();
            var ay = random.NextDouble();
            var az = random.NextDouble();
            var len = Math.Sqrt(ax * ax + ay * ay + az * az);
            ax /= len;
            ay /= len;
            az /= len;
            SetRotationAngle(ax, ay, az, random.NextDouble() * Math.PI * 2.0);
            var angle = random.NextDouble() * Math.PI * 2.0;
            cos2D = Math.Cos(angle);
            sin2D = Math.Sin(angle);
        }
    }

    public BasisType BasisType
    {
        get => basisType;
        set
        {
            basisType = value;
            switch (basisType)
            {
                case BasisType.Value:
                    noise2D = Noise.ValueNoise;
                    noise3D = Noise.ValueNoise;
                    noise4D = Noise.ValueNoise;
                    noise6D = Noise.ValueNoise;
                    break;
                case BasisType.Gradient:
                    noise2D = Noise.GradientNoise;
                    noise3D = Noise.GradientNoise;
                    noise4D = Noise.GradientNoise;
                    noise6D = Noise.GradientNoise;
                    break;
                case BasisType.GradientValue:
                    noise2D = Noise.GradientValueNoise;
                    noise3D = Noise.GradientValueNoise;
                    noise4D = Noise.GradientValueNoise;
                    noise6D = Noise.GradientValueNoise;
                    break;
                case BasisType.White:
                    noise2D = Noise.WhiteNoise;
                    noise3D = Noise.WhiteNoise;
                    noise4D = Noise.WhiteNoise;
                    noise6D = Noise.WhiteNoise;
                    break;
                case BasisType.Simplex:
                    noise2D = Noise.SimplexNoise;
                    noise3D = Noise.SimplexNoise;
                    noise4D = Noise.SimplexNoise;
                    noise6D = Noise.SimplexNoise;
                    break;

                default:
                    noise2D = Noise.GradientNoise;
                    noise3D = Noise.GradientNoise;
                    noise4D = Noise.GradientNoise;
                    noise6D = Noise.GradientNoise;
                    break;
            }

            SetMagicNumbers(basisType);
        }
    }

    public InterpolationType InterpolationType
    {
        get => interpolationType;
        set
        {
            interpolationType = value;
            switch (interpolationType)
            {
                case InterpolationType.None:
                    interpolator = Noise.NoInterpolation;
                    break;
                case InterpolationType.Linear:
                    interpolator = Noise.LinearInterpolation;
                    break;
                case InterpolationType.Cubic:
                    interpolator = Noise.HermiteInterpolation;
                    break;
                default:
                    interpolator = Noise.QuinticInterpolation;
                    break;
            }
        }
    }

    public override double Get(double x, double y)
    {
        var nx = x * cos2D - y * sin2D;
        var ny = y * cos2D + x * sin2D;

        return noise2D(nx, ny, seed, interpolator);
    }

    public override double Get(double x, double y, double z)
    {
        var nx = rotationMatrix[0, 0] * x + rotationMatrix[1, 0] * y + rotationMatrix[2, 0] * z;
        var ny = rotationMatrix[0, 1] * x + rotationMatrix[1, 1] * y + rotationMatrix[2, 1] * z;
        var nz = rotationMatrix[0, 2] * x + rotationMatrix[1, 2] * y + rotationMatrix[2, 2] * z;

        return noise3D(nx, ny, nz, seed, interpolator);
    }

    public override double Get(double x, double y, double z, double w)
    {
        var nx = rotationMatrix[0, 0] * x + rotationMatrix[1, 0] * y + rotationMatrix[2, 0] * z;
        var ny = rotationMatrix[0, 1] * x + rotationMatrix[1, 1] * y + rotationMatrix[2, 1] * z;
        var nz = rotationMatrix[0, 2] * x + rotationMatrix[1, 2] * y + rotationMatrix[2, 2] * z;

        return noise4D(nx, ny, nz, w, seed, interpolator);
    }

    public override double Get(double x, double y, double z, double w, double u, double v)
    {
        var nx = rotationMatrix[0, 0] * x + rotationMatrix[1, 0] * y + rotationMatrix[2, 0] * z;
        var ny = rotationMatrix[0, 1] * x + rotationMatrix[1, 1] * y + rotationMatrix[2, 1] * z;
        var nz = rotationMatrix[0, 2] * x + rotationMatrix[1, 2] * y + rotationMatrix[2, 2] * z;

        return noise6D(nx, ny, nz, w, u, v, seed, interpolator);
    }

    private void SetRotationAngle(double x, double y, double z, double angle)
    {
        rotationMatrix[0, 0] = 1 + (1 - Math.Cos(angle)) * (x * x - 1);
        rotationMatrix[1, 0] = -z * Math.Sin(angle) + (1 - Math.Cos(angle)) * x * y;
        rotationMatrix[2, 0] = y * Math.Sin(angle) + (1 - Math.Cos(angle)) * x * z;

        rotationMatrix[0, 1] = z * Math.Sin(angle) + (1 - Math.Cos(angle)) * x * y;
        rotationMatrix[1, 1] = 1 + (1 - Math.Cos(angle)) * (y * y - 1);
        rotationMatrix[2, 1] = -x * Math.Sin(angle) + (1 - Math.Cos(angle)) * y * z;

        rotationMatrix[0, 2] = -y * Math.Sin(angle) + (1 - Math.Cos(angle)) * x * z;
        rotationMatrix[1, 2] = x * Math.Sin(angle) + (1 - Math.Cos(angle)) * y * z;
        rotationMatrix[2, 2] = 1 + (1 - Math.Cos(angle)) * (z * z - 1);
    }

    private void SetMagicNumbers(BasisType type)
    {
        // This function is a damned hack.
        // The underlying noise functions don't return values in the range [-1,1] cleanly, and the ranges vary depending
        // on basis type and dimensionality. There's probably a better way to correct the ranges, but for now I'm just
        // setting he magic numbers scale and offset manually to empirically determined magic numbers.
        switch (type)
        {
            case BasisType.Value:
                scale[0] = 1.0;
                offset[0] = 0.0;
                scale[1] = 1.0;
                offset[1] = 0.0;
                scale[2] = 1.0;
                offset[2] = 0.0;
                scale[3] = 1.0;
                offset[3] = 0.0;
                break;

            case BasisType.Gradient:
                scale[0] = 1.86848;
                offset[0] = -0.000118;
                scale[1] = 1.85148;
                offset[1] = -0.008272;
                scale[2] = 1.64127;
                offset[2] = -0.01527;
                scale[3] = 1.92517;
                offset[3] = 0.03393;
                break;

            case BasisType.GradientValue:
                scale[0] = 0.6769;
                offset[0] = -0.00151;
                scale[1] = 0.6957;
                offset[1] = -0.133;
                scale[2] = 0.74622;
                offset[2] = 0.01916;
                scale[3] = 0.7961;
                offset[3] = -0.0352;
                break;

            case BasisType.White:
                scale[0] = 1.0;
                offset[0] = 0.0;
                scale[1] = 1.0;
                offset[1] = 0.0;
                scale[2] = 1.0;
                offset[2] = 0.0;
                scale[3] = 1.0;
                offset[3] = 0.0;
                break;

            default:
                scale[0] = 1.0;
                offset[0] = 0.0;
                scale[1] = 1.0;
                offset[1] = 0.0;
                scale[2] = 1.0;
                offset[2] = 0.0;
                scale[3] = 1.0;
                offset[3] = 0.0;
                break;
        }
    }
}
