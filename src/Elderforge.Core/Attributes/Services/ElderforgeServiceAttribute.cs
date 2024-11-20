namespace Elderforge.Core.Server.Attributes.Services;

[AttributeUsage(AttributeTargets.Class)]
public class ElderforgeServiceAttribute : Attribute
{
    public int Priority { get; }

    public ElderforgeServiceAttribute(int priority = 0)
    {
        Priority = priority;
    }
}
