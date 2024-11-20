using Elderforge.Core.Interfaces.Events;
using Elderforge.Core.Server.Data.TimeWeather;

namespace Elderforge.Core.Server.Events.World.TimeAndWeather;

public record TimeChangeEvent(TimeOfDayData Time) : IElderforgeEvent;
