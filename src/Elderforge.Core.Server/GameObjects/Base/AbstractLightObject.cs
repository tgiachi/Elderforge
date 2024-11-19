using System.Reactive.Subjects;
using Elderforge.Shared.Interfaces;
using Elderforge.Shared.Types;

namespace Elderforge.Core.Server.GameObjects.Base;

public class AbstractLightObject : AbstractGameObject, ILightGameObject
{
    public event ILightGameObject.LightIntensityHandler? LightIntensityChanged;
    public event ILightGameObject.LightColorHandler? LightColorChanged;

    public ISubject<ILightGameObject.LightIntensityHandler> LightIntensitySubject { get; }
    public ISubject<ILightGameObject.LightColorHandler> LightColorSubject { get; }

    public LightType LightType { get; set; }
    public float LightIntensity { get; set; }
    public string LightColor { get; set; }


    public AbstractLightObject()
    {
        LightIntensitySubject = new Subject<ILightGameObject.LightIntensityHandler>();
        LightColorSubject = new Subject<ILightGameObject.LightColorHandler>();


        PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(LightIntensity))
            {
                LightIntensityChanged?.Invoke(this, LightIntensity);
                LightIntensitySubject.OnNext(LightIntensityChanged);
            }
            else if (args.PropertyName == nameof(LightColor))
            {
                LightColorChanged?.Invoke(this, LightColor);
                LightColorSubject.OnNext(LightColorChanged);
            }
        };
    }

    public override string ToString()
    {
        return $"LightType: {LightType}, LightIntensity: {LightIntensity}, LightColor: {LightColor}";
    }
}
