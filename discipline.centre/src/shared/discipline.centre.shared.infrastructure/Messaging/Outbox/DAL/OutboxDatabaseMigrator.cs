using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace discipline.centre.shared.infrastructure.Messaging.Outbox.DAL;

internal sealed class OutboxDatabaseMigrator(
    ILogger<OutboxDatabaseMigrator> logger,
    IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OutboxDbContext>();

        try
        {
            await dbContext.Database.MigrateAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError("An error occured during migration");
            logger.LogCritical(ex.Message);
            throw;
        }
        
        logger.LogInformation("Migration succeeded");
    }

    public Task StopAsync(CancellationToken cancellationToken)
        =>  Task.CompletedTask;
}