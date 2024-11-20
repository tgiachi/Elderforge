using Elderforge.Core.Interfaces.Services;
using Elderforge.Core.Server.Data.TimeWeather;
using Elderforge.Core.Server.Events.World.TimeAndWeather;
using Elderforge.Core.Server.Interfaces.Services.Game;
using Elderforge.Core.Server.Interfaces.Services.Game.Base;
using Elderforge.Core.Server.Interfaces.Services.System;
using Elderforge.Core.Server.Types;
using Serilog;

namespace Elderforge.Server.Services.Game;

public class TimeAndWeatherService : AbstractGameService, ITimeAndWeatherService
{
    private const float REAL_MINUTES_PER_GAME_DAY = 60f;
    private const float GAME_MINUTES_PER_DAY = 1440f;
    private static readonly int _schedulerMsTick = 2000;

    private DayPhase lastPhase;

    private readonly float _deltaMinutes = (float)TimeSpan.FromMilliseconds(_schedulerMsTick).TotalMinutes;

    private float currentGameTime;

    private readonly ILogger _logger = Log.ForContext<TimeAndWeatherService>();



    public TimeAndWeatherService(ISchedulerService schedulerService, IEventBusService eventBusService) : base(
        eventBusService
    )
    {
        schedulerService.AddSchedulerJob("timeOfDay", TimeSpan.FromMilliseconds(_schedulerMsTick), OnSchedulerTick);
    }

    private async Task OnSchedulerTick()
    {
        var gameMinutesElapsed = (_deltaMinutes / REAL_MINUTES_PER_GAME_DAY) * GAME_MINUTES_PER_DAY;
        currentGameTime = (currentGameTime + gameMinutesElapsed) % GAME_MINUTES_PER_DAY;

        var timeData = CalculateTimeOfDay();

        await CheckPhaseChange(timeData);

        _logger.Debug("Current Time: {Time}", timeData);
        await SendEventAsync(new TimeChangeEvent(timeData));
    }


    private TimeOfDayData CalculateTimeOfDay()
    {
        int hours = (int)(currentGameTime / 60f);
        int minutes = (int)(currentGameTime % 60f);

        return new TimeOfDayData
        {
            Hours = hours,
            Minutes = minutes,
            NormalizedTime = currentGameTime / GAME_MINUTES_PER_DAY,
            Phase = GetDayPhase(hours),
            IsDayTime = IsDayTime(hours)
        };
    }

    private async Task CheckPhaseChange(TimeOfDayData timeData)
    {
        if (timeData.Phase != lastPhase)
        {
            lastPhase = timeData.Phase;
            _logger.Debug("Day phase changed to {Phase}", timeData.Phase);
            await SendEventAsync(new DayPhaseChangeEvent(lastPhase));
        }
    }

    private static DayPhase GetDayPhase(int hours)
    {
        return hours switch
        {
            >= 5 and < 7   => DayPhase.Dawn,
            >= 7 and < 17  => DayPhase.Day,
            >= 17 and < 19 => DayPhase.Dusk,
            _              => DayPhase.Night
        };
    }

    private static bool IsDayTime(int hours)
    {
        return hours is >= 6 and < 18;
    }
}
