namespace Elderforge.Core.Server.Attributes.Services;

[AttributeUsage(AttributeTargets.Class)]
public class ElderforgeServiceAttribute : Attribute
{
    public int Order { get; }

    public ElderforgeServiceAttribute(int order = 0)
    {
        Order = order;
    }
}
