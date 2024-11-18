using Elderforge.Core.Server.Noise.AccidentalNoise.Enums;
using Elderforge.Core.Server.Noise.AccidentalNoise.Implicit;

namespace Elderforge.Core.Server.Noise;

public class FastNoiseLite
{
    private NoiseType noiseType;

    private readonly ImplicitFractal _implicitFractal;


    public enum NoiseType
    {
        Perlin
    }

    public FastNoiseLite()
    {

    }

    public void SetSeed(int seed)
    {
        _implicitFractal.Seed = seed;
    }

    public void SetNoiseType(NoiseType type)
    {
        this.noiseType = type;
    }

    public float GetNoise(float x, float y)
    {
        return (float)_implicitFractal.Get(x, y);
    }

    public float GetNoise(float x, float y, float z)
    {
        return (float)_implicitFractal.Get(x, y, z);
    }
}
