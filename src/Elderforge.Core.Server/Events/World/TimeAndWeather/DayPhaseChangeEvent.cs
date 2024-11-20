using Elderforge.Core.Interfaces.Events;
using Elderforge.Core.Server.Types;
using Elderforge.Shared.Types;

namespace Elderforge.Core.Server.Events.World.TimeAndWeather;

public record DayPhaseChangeEvent(DayPhase Phase) : IElderforgeEvent;
