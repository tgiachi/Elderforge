namespace Elderforge.Core.Interfaces.Services.Base;

public interface IElderforgeService
{
    Task StartAsync();

    Task StopAsync();
}
