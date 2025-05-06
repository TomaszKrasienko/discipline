using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace discipline.hangfire.infrastructure.Postgres;

internal sealed class DatabaseMigrator<TContext>(
    ILogger<DatabaseMigrator<TContext>> logger,
    IServiceProvider serviceProvider) : IHostedService where TContext : DbContext
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();

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
        => Task.CompletedTask;
}