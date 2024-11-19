using System.ComponentModel;
using System.Numerics;
using System.Reactive.Subjects;
using Elderforge.Shared.Interfaces;
using Elderforge.Shared.Types;

namespace Elderforge.Core.Server.GameObjects.Base;

public class AbstractGameObject : IGameObject, INotifyPropertyChanged
{
    public event IGameObject.Vector3ChangedEventHandler? PositionChanged;
    public event IGameObject.Vector3ChangedEventHandler? RotationChanged;
    public event IGameObject.Vector3ChangedEventHandler? ScaleChanged;
    public ISubject<IGameObject.Vector3ChangedEventHandler> PositionSubject { get; }
    public ISubject<IGameObject.Vector3ChangedEventHandler> RotationSubject { get; }
    public ISubject<IGameObject.Vector3ChangedEventHandler> ScaleSubject { get; }

    public string Id { get; set; }

    public GameObjectType ObjectType { get; set; }

    public string Name { get; set; }


    public Vector3 Position { get; set; }

    public Vector3 Rotation { get; set; }

    public Vector3 Scale { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;


    public AbstractGameObject(GameObjectType objectType)
    {
        ObjectType = objectType;
        Position = Vector3.Zero;

        Rotation = Vector3.Zero;

        Scale = Vector3.One;

        PropertyChanged += OnPropertyChanged;

        PositionSubject = new Subject<IGameObject.Vector3ChangedEventHandler>();
        RotationSubject = new Subject<IGameObject.Vector3ChangedEventHandler>();
        ScaleSubject = new Subject<IGameObject.Vector3ChangedEventHandler>();
    }

    protected void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(Position):
                PositionChanged?.Invoke(this, Position);
                PositionSubject?.OnNext(PositionChanged);
                break;
            case nameof(Rotation):
                RotationChanged?.Invoke(this, Rotation);
                RotationSubject?.OnNext(RotationChanged);
                break;
            case nameof(Scale):
                ScaleChanged?.Invoke(this, Scale);
                ScaleSubject?.OnNext(ScaleChanged);
                break;
        }
    }

    public override string ToString()
    {
        return $"{Name} ({Id}) - {ObjectType} - Position: {Position}, Rotation: {Rotation}, Scale: {Scale}";
    }
}
