using Elderforge.Core.Server.Attributes.Scripts;
using Elderforge.Core.Server.Interfaces.Services.System;

namespace Elderforge.Server.ScriptModules;


[ScriptModule]
public class AccountModule
{
    private readonly IAccountService _accountService;

    public AccountModule(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [ScriptFunction("add_admin")]
    public void AddAdmin(string username, string password)
    {
        _accountService.AddAdmin(username, password);
    }

}
