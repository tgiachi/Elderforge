using Elderforge.Core.Server.Types;

namespace Elderforge.Core.Server.Data.TimeWeather;

public struct TimeOfDayData
{
    public int Hours { get; set; }
    public int Minutes { get; set; }
    public float NormalizedTime { get; set; }
    public DayPhase Phase { get; set; }
    public bool IsDayTime { get; set; }

    public override string ToString()
    {
        return $"{Hours:00}:{Minutes:00} ({Phase})";
    }
}
