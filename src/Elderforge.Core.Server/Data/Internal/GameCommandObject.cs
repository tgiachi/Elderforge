namespace Elderforge.Core.Server.Data.Internal;


public record GameCommandObject(string Command, string Description, string Help, string[] Permissions)
{
}
