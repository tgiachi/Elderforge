using System.Numerics;

namespace Elderforge.Shared.Interfaces;

public interface IGameObject
{
    string Id { get; set; }

    Vector3 Position { get; set; }

    Vector3 Rotation { get; set; }

    Vector3 Scale { get; set; }
}
