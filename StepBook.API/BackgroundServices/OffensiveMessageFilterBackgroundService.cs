namespace StepBook.API.BackgroundServices;

public class OffensiveMessageFilterBackgroundService(IServiceProvider provider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }
}