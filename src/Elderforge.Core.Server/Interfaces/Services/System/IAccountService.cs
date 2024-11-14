using Elderforge.Core.Interfaces.Services.Base;

namespace Elderforge.Core.Server.Interfaces.Services.System;

public interface IAccountService : IElderforgeService
{
    void AddAdmin(string username, string password);

    Task<Guid> LoginAsync(string username, string password);

}
