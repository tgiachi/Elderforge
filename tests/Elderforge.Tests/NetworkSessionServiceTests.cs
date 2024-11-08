using Elderforge.Network.Data.Session;
using Elderforge.Network.Services;

namespace Elderforge.Tests;

public class NetworkSessionServiceTests
{
    [Fact]
    public void TestNonExistSession()
    {
        var sessionService = new NetworkSessionService();

        var session = sessionService.GetSessionObject("test");

        Assert.Null(session);
    }

    [Fact]
    public void TestExistSession()
    {
        var sessionService = new NetworkSessionService();

        var session = new SessionObject(null);


        sessionService.AddSession("test", session);

        var session2 = sessionService.GetSessionObject("test");

        Assert.NotNull(session2);
    }
}
