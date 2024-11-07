using Elderforge.Core.Interfaces.Events;

namespace Elderforge.Tests.Data;

public record DummyEvent(int Id) : IElderforgeEvent;
