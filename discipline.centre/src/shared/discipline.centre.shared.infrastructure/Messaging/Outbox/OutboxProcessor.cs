using discipline.centre.shared.infrastructure.Messaging.Abstractions;
using discipline.centre.shared.infrastructure.Messaging.Outbox.Configuration.Options;
using discipline.centre.shared.infrastructure.Messaging.Outbox.DAL;
using discipline.libs.messaging.Abstractions;
using discipline.libs.serializers.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;

namespace discipline.centre.shared.infrastructure.Messaging.Outbox;

internal sealed class OutboxProcessor(
    ILogger<OutboxProcessor> logger,
    IOptions<OutboxOptions> options,
    ISerializer serializer,
    TimeProvider timeProvider,
    IServiceProvider serviceProvider) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Outbox processor started");

        try
        {
            using var scope = serviceProvider.CreateScope();
            var outboxContext = scope.ServiceProvider.GetRequiredService<OutboxDbContext>();
            var producers = scope.ServiceProvider.GetRequiredService<IEnumerable<IMessagePublisher>>()
                .Where(x => !x.IsOutbox())
                .ToList();

            var outboxMessages = await outboxContext
                .OutboxMessages
                .Where(x
                    => x.RetryCount >= options.Value.RetryCount
                       && x.SentAt == null)
                .ToListAsync(context.CancellationToken);

            foreach (var outboxMessage in outboxMessages)
            {
                var transaction = await outboxContext.Database.BeginTransactionAsync(context.CancellationToken);
                try
                {
                    var type = GetTypeByName(outboxMessage.MessageType);

                    if (type is null)
                    {
                        return;
                    }

                    var serializedMessage = serializer.ToObject(
                        outboxMessage.JsonContent,
                        type);

                    if (serializedMessage is IMessage message)
                    {
                        var tasks = producers
                            .Select(x => x.PublishAsync(
                                message,
                                outboxMessage.MessageId,
                                context.CancellationToken));

                        await Task.WhenAll(tasks);

                        outboxMessage.SetSentAt(timeProvider.GetUtcNow());
                    }

                    await transaction.CommitAsync(context.CancellationToken);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(context.CancellationToken);
                    outboxMessage.IncreaseRetryCount();
                    logger.LogError(ex, ex.Message);
                }
                finally
                {
                    await outboxContext.SaveChangesAsync(context.CancellationToken);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
        }
    }
    
    public static Type? GetTypeByName(string fullTypeName)
    {
        var type = Type.GetType(fullTypeName);
        if (type != null)
            return type;

        // Jeśli nie znaleziono, przeszukujemy wszystkie załadowane assembly
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            type = assembly.GetType(fullTypeName);
            if (type != null)
                return type;
        }

        // Można też spróbować załadować assembly dynamicznie po nazwie, jeśli jest znana
        return null; // Nie znaleziono typu
    }
}