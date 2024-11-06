using System.Diagnostics;

namespace Elderforge.Core.Utils;

public static class StopwatchUtils
{
    public static double GetElapsedMilliseconds(long startTime, long endTime) =>
        (endTime - startTime) / (double)Stopwatch.Frequency * 1000;
}
