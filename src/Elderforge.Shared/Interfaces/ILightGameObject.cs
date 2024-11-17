using Elderforge.Shared.Types;

namespace Elderforge.Shared.Interfaces;

public interface ILightGameObject : IGameObject
{
    LightType LightType { get; set; }

    float LightIntensity { get; set; }

    string LightColor { get; set; }
}
