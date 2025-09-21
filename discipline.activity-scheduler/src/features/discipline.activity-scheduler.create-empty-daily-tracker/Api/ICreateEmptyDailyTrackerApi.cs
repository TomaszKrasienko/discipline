using discipline.activity_scheduler.create_empty_daily_tracker.Commands;
using discipline.activity_scheduler.shared.abstractions.DAL;
using discipline.activity_scheduler.shared.abstractions.Identifiers;
using discipline.activity_scheduler.shared.abstractions.Messaging;
using discipline.activity_scheduler.shared.abstractions.ViewModels;

namespace discipline.activity_scheduler.create_empty_daily_tracker.Api;

public interface ICreateEmptyDailyTrackerApi
{
    Task Generate(CancellationToken cancellationToken = default);
}

internal sealed class CreateEmptyDailyTrackerApi(
    TimeProvider timeProvider,
    IMessageProcessor messageProcessor,
    IReadRepository<AccountViewModel, AccountId> readAccountRepository,
    IReadRepository<PlannedTaskViewModel, PlannedTaskId> readPlannedTaskRepository) : ICreateEmptyDailyTrackerApi
{
    public async Task Generate(CancellationToken cancellationToken = default)
    {
        var now = DateOnly.FromDateTime(timeProvider.GetUtcNow().DateTime);
        
        var accounts = await readAccountRepository
            .GetAllAsync(cancellationToken);
        
        var plannedTask = await readPlannedTaskRepository
            .SearchAsync(
                x => x.PlannedFor == now,
                cancellationToken);

        var accountWithPlanned = plannedTask.Select(x => x.AccountId.Value.ToString())
            .Distinct()
            .ToList();

        var accountsWithoutPlanned = accounts
            .Where(x => !accountWithPlanned.Contains(x.AccountId))
            .ToList();

        var commands = accountsWithoutPlanned
            .Select(x => new CreateEmptyUserDailyTrackerCommand(x.AccountId, now))
            .ToList();
        
        var messageTasks = commands
            .Select(command => messageProcessor.SendAsync(command, cancellationToken))
            .ToList();
        
        await Task.WhenAll(messageTasks);
    }
}