using System.ComponentModel;
using System.Numerics;
using Elderforge.Shared.Interfaces;

namespace Elderforge.Core.Server.GameObjects.Base;

public class AbstractGameObject : IGameObject, INotifyPropertyChanged
{
    public event IGameObject.Vector3ChangedEventHandler? PositionChanged;

    public event IGameObject.Vector3ChangedEventHandler? RotationChanged;

    public event IGameObject.Vector3ChangedEventHandler? ScaleChanged;

    public string Id { get; set; }

    public string Name { get; set; }


    public Vector3 Position { get; set; }

    public Vector3 Rotation { get; set; }

    public Vector3 Scale { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;


    public AbstractGameObject()
    {
        Position = Vector3.Zero;

        Rotation = Vector3.Zero;

        Scale = Vector3.One;

        PropertyChanged += OnPropertyChanged;
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(Position):
                PositionChanged?.Invoke(Position);
                break;
            case nameof(Rotation):
                RotationChanged?.Invoke(Rotation);
                break;
            case nameof(Scale):
                ScaleChanged?.Invoke(Scale);
                break;
        }
    }

    public override string ToString()
    {
        return $"{Name} ({Id})";
    }
}
