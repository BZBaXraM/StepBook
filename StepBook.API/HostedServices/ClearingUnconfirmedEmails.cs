using StepBook.DAL.Data;

namespace StepBook.API.HostedServices;

/// <summary>
/// Clearing unconfirmed emails
/// </summary>
/// <param name="logger"></param>
/// <param name="serviceProvider"></param>
public class ClearingUnconfirmedEmails(ILogger<ClearingUnconfirmedEmails> logger, IServiceProvider serviceProvider)
    : IHostedService
{
    private bool _isRunning;

    private async Task RunAsync(CancellationToken cancellationToken)
    {
        if (_isRunning)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<StepContext>();

            var unconfirmedEmails = await dbContext.Users
                .Where(u => u.IsEmailConfirmed == false)
                .ToListAsync(cancellationToken: cancellationToken);


            foreach (var user in unconfirmedEmails)
            {
                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);

                logger.LogInformation("User {Email} has not confirmed his email. Deleting...", user.Email);

                logger.LogCritical("User {Email} has not confirmed his email. Deleting...", user.Email);

                dbContext.Users.Remove(user);
            }


            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Start the hosted service
    /// </summary>
    /// <param name="cancellationToken"></param>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _isRunning = true;
        await RunAsync(cancellationToken);
    }

    /// <summary>
    /// Stop the hosted service
    /// </summary>
    /// <param name="cancellationToken"></param>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _isRunning = false;
        await Task.CompletedTask;
    }
}