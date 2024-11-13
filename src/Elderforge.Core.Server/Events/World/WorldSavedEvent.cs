using Elderforge.Core.Interfaces.Events;

namespace Elderforge.Core.Server.Events.World;

public record WorldSavedEvent(string FileName) : IElderforgeEvent;
