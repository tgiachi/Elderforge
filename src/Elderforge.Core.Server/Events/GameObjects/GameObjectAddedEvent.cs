using Elderforge.Core.Interfaces.Events;

namespace Elderforge.Core.Server.Events.GameObjects;

public record GameObjectAddedEvent(string Id) : IElderforgeEvent;
