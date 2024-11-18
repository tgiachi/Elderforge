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
            .Select(sessionService.GetSessionObject)
            .Where(x => x != null)
            .Where(
                x =>
                {
                    var playerPosition = x.GetPosition();
                    var maxDistance = renderDistance * renderDistance;
                    var distanceSquared = (playerPosition - position).SqrMagnitude();

                    return distanceSquared <= maxDistance;
                }
            )
            .ToList();
    }

    public static Vector3 GetPosition(this ISessionObject sessionObject)
    {
        return sessionObject.GetDataObject<Vector3>("position");
    }

    public static Vector3 GetRotation(this ISessionObject sessionObject)
    {
        return sessionObject.GetDataObject<Vector3>("rotation");
    }

    public static void SetPosition(this ISessionObject sessionObject, Vector3 position)
    {
        sessionObject.SetDataObject("position", position);
    }

    public static void SetRotation(this ISessionObject sessionObject, Vector3 rotation)
    {
        sessionObject.SetDataObject("rotation", rotation);
    }
}
