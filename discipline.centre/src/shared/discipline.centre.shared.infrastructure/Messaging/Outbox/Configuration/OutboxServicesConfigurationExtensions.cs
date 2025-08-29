using discipline.centre.shared.infrastructure.Messaging.Outbox;
using discipline.centre.shared.infrastructure.Messaging.Outbox.Configuration.Options;
using discipline.centre.shared.infrastructure.Messaging.Outbox.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Quartz;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class OutboxServicesConfigurationExtensions
{
    internal static IServiceCollection AddOutbox(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions(configuration);
        var outboxOptions = services.GetOptions<OutboxOptions>();

        if (outboxOptions.IsEnabled)
        {
            services.AddContext();
            services.AddJobs();
        }
        
        return services;
    }
    
    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        => services.Configure<OutboxOptions>(configuration.GetSection(nameof(OutboxOptions)));

    private static IServiceCollection AddContext(this IServiceCollection services)
    {
        var outboxOptions = services.GetOptions<OutboxOptions>();

        if (string.IsNullOrWhiteSpace(outboxOptions.ConnectionString))
        {
            throw new InvalidOperationException("No connection string configured");
        }

        services.AddDbContext<OutboxDbContext>(x => x.UseNpgsql(
            outboxOptions.ConnectionString));

        services.AddHostedService<OutboxDatabaseMigrator>();
        
        return services;
    }

    private static IServiceCollection AddJobs(this IServiceCollection services)
    {
        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        services.AddQuartz(options =>
        {
            var jobKey = new JobKey($"outbox-processor-{Guid.NewGuid()}", "outbox-processor");

            options.AddJob<OutboxProcessor>(opt =>
                opt.WithIdentity(jobKey));

            options.AddTrigger(opt =>
                opt.ForJob(jobKey)
                    .WithCronSchedule("0/20 * * * * ?"));
        });
        
        return services;
    }
}