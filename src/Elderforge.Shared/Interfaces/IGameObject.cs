using System.Numerics;

namespace Elderforge.Shared.Interfaces;

public interface IGameObject
{
    delegate void Vector3ChangedEventHandler(Vector3 newValue);

    event Vector3ChangedEventHandler? PositionChanged;

    event Vector3ChangedEventHandler? RotationChanged;

    event Vector3ChangedEventHandler? ScaleChanged;

    string Id { get; set; }

    string Name { get; set; }

    Vector3 Position { get; set; }

    Vector3 Rotation { get; set; }

    Vector3 Scale { get; set; }
}
