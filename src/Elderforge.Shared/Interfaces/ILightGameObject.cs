using System.ComponentModel;
using System.Reactive.Subjects;
using Elderforge.Shared.Types;

namespace Elderforge.Shared.Interfaces;

public interface ILightGameObject : IGameObject, INotifyPropertyChanged
{
    delegate void LightIntensityHandler(object sender, float intensity);

    delegate void LightColorHandler(object sender, string color);


    event LightIntensityHandler LightIntensityChanged;

    event LightColorHandler LightColorChanged;


    ISubject<LightIntensityHandler> LightIntensitySubject { get; }

    ISubject<LightColorHandler> LightColorSubject { get; }

    LightType LightType { get; set; }

    float LightIntensity { get; set; }

    string LightColor { get; set; }
}
