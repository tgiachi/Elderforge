using Elderforge.Core.Server.Attributes.Services;
using Elderforge.Core.Server.Interfaces.Services.System;
using Elderforge.Core.Server.Utils;
using Elderforge.Entities.Database;
using Elderforge.Network.Data.Internal;
using Elderforge.Network.Interfaces.Listeners;
using Elderforge.Network.Interfaces.Services;
using Elderforge.Network.Packets.Login;
using Serilog;

namespace Elderforge.Server.Services.System;


[ElderforgeService]
public class AccountService : IAccountService, INetworkMessageListener<LoginRequestMessage>
{
    private readonly ILogger _logger = Log.ForContext<AccountService>();

    private readonly INetworkSessionService _networkSessionService;

    private readonly List<Admin> _admins = new();

    private readonly IDatabaseService _databaseService;

    public AccountService(IDatabaseService databaseService, INetworkSessionService networkSessionService)
    {
        _databaseService = databaseService;
        _networkSessionService = networkSessionService;
    }

    public void AddAdmin(string username, string password)
    {
        _admins.Add(new Admin(username, password));
    }

    public async Task<Guid> LoginAsync(string username, string password)
    {
        var users = await _databaseService.QueryAsync<UserEntity>(s => s.Email == username);

        if (users.Any())
        {
            var userEntity = users.First();

            if (userEntity.PasswordHash == password.Sha1Hash())
            {
                return userEntity.Id;
            }
        }

        return Guid.Empty;
    }

    public async Task StartAsync()
    {
        foreach (var admin in _admins)
        {
            var exists = await _databaseService.QueryAsync<UserEntity>(s => s.Email == admin.Username);

            if (!exists.Any())
            {
                _logger.Information("Creating admin account for {Username}", admin.Username);

                await _databaseService.InsertAsync(
                    new UserEntity
                    {
                        Email = admin.Username,
                        PasswordHash = admin.Password.Sha1Hash()
                    }
                );
            }
        }
    }

    public Task StopAsync()
    {
        return Task.CompletedTask;
    }

    public async ValueTask<IEnumerable<SessionNetworkMessage>> OnMessageReceivedAsync(
        string sessionId, LoginRequestMessage message
    )
    {
        var userId = await LoginAsync(message.Username, message.Password);
        var loginResponse = new LoginResponseMessage();

        if (userId != Guid.Empty)
        {
            _logger.Information("User {Username} logged in from sessionId: {Session}", message.Username, sessionId);
            loginResponse.Success = true;
            loginResponse.UserId = userId.ToString();

            _networkSessionService.GetSessionObject(sessionId).IsLoggedIn = true;
        }
        else
        {
            loginResponse.Success = false;
        }

        return new[]
        {
            new SessionNetworkMessage(sessionId, loginResponse)
        };
    }
}

internal record Admin(string Username, string Password);
