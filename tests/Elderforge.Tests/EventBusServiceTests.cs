using Elderforge.Core.Services;
using Elderforge.Tests.Data;

namespace Elderforge.Tests;

public class EventBusServiceTests
{
    [Fact]
    public void TestSendAndReceivedMessage()
    {
        var eventBus = new EventBusService();

        var received = 0;

        eventBus.Subscribe<DummyEvent>(e => { received++; });

        eventBus.Publish(new DummyEvent(1));

        Assert.Equal(1, received);
    }
}
