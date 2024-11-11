namespace Elderforge.Shared.Services;

public class ElderforgeInstanceHolder
{
    public static ElderforgeInstanceHolder Instance { get; private set; }

    public ElderforgeInstanceHolder()
    {
        Instance = this;
    }
}
