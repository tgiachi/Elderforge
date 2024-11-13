using Elderforge.Core.Interfaces.Services.Base;
using Elderforge.Core.Server.Data.Internal;

namespace Elderforge.Core.Server.Interfaces.Services.Game;

public interface IGameCommandService : IElderforgeService
{
    void RegisterCommand(
        string command, string description, string help, Action<GameCommandContext> action
    );
}
