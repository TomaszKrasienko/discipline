using discipline.activity_scheduler.create_activity_from_planned.Commands;
using discipline.activity_scheduler.shared.abstractions.DAL;
using discipline.activity_scheduler.shared.abstractions.Identifiers;
using discipline.activity_scheduler.shared.abstractions.Messaging;
using discipline.activity_scheduler.shared.abstractions.ViewModels;

namespace discipline.activity_scheduler.create_activity_from_planned.Apis;

public interface ICreateActivitiesFromPlanned
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}

internal sealed class CreateActivitiesFromPlanned(
    IReadRepository<PlannedTaskViewModel, PlannedTaskId> readPlannedTasksRepository,
    TimeProvider timeProvider,
    IMessageProcessor messageProcessor) : ICreateActivitiesFromPlanned
{
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var now = timeProvider.GetUtcNow();
        
        var plannedTasks = await readPlannedTasksRepository
            .SearchAsync(
                x => 
                    x.PlannedFor == DateOnly.FromDateTime(now.DateTime) &&
                    x.PlannedEnable,
                cancellationToken);
        
        var tasksPerAccount = plannedTasks
            .GroupBy(x => x.AccountId)
            .ToDictionary(x => x.Key, x => x.ToList());

        List<CreateActivityForAccountFromActivityRuleCommand> commands = [];
        
        foreach (var plannedTask in tasksPerAccount)
        {
            commands.Add(new CreateActivityForAccountFromActivityRuleCommand(
                plannedTask.Value.Select(x => x.ActivityRuleId.ToString()).ToList(),
                plannedTask.Key.ToString()));
        }

        var sendCommandTasks = commands
            .Select(command => messageProcessor.SendAsync(command, cancellationToken));
        
        await Task.WhenAll(sendCommandTasks);
    }
}