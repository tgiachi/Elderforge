using System.Numerics;
using Elderforge.Core.Extensions;
using Elderforge.Network.Interfaces.Services;
using Elderforge.Network.Interfaces.Sessions;

namespace Elderforge.Server.Extensions;

public static class NetworkSessionExtension
{
    public static IEnumerable<ISessionObject> GetSessionObjectCanSee(
        this INetworkSessionService sessionService, float renderDistance, Vector3 position
    )
    {
        return sessionService.GetSessionIds
            .Select(x => sessionService.GetSessionObject(x))
            .Where(x => x != null)
            .Where(
                x =>
                {
                    var playerPosition = x.GetDataObject<Vector3>("position");
                    var maxDistance = renderDistance * renderDistance;
                    var distanceSquared = (playerPosition - position).SqrMagnitude();

                    return distanceSquared <= maxDistance;
                }
            )
            .ToList();
    }
}
