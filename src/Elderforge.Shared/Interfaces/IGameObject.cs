using System.Numerics;
using System.Reactive.Subjects;
using Elderforge.Shared.Types;

namespace Elderforge.Shared.Interfaces;

public interface IGameObject
{
    delegate void Vector3ChangedEventHandler(IGameObject gameObject, Vector3 newValue);

    event Vector3ChangedEventHandler? PositionChanged;

    event Vector3ChangedEventHandler? RotationChanged;

    event Vector3ChangedEventHandler? ScaleChanged;


    ISubject<Vector3ChangedEventHandler> PositionSubject { get; }

    ISubject<Vector3ChangedEventHandler> RotationSubject { get; }

    ISubject<Vector3ChangedEventHandler> ScaleSubject { get; }


    string Id { get; set; }

    GameObjectType ObjectType { get; set; }

    string Name { get; set; }

    Vector3 Position { get; set; }

    Vector3 Rotation { get; set; }

    Vector3 Scale { get; set; }
}
