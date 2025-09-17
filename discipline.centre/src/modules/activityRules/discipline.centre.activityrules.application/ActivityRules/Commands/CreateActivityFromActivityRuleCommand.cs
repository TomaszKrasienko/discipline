using discipline.centre.activityrules.domain.Repositories;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.libs.cqrs.abstractions.Commands;

namespace discipline.centre.activityrules.application.ActivityRules.Commands;

public sealed record CreateActivitiesFromActivityRuleCommand(
    IReadOnlyCollection<ActivityRuleId> ActivityRuleIds,
    AccountId AccountId) : ICommand;
    
internal sealed class CreateActivityFromActivityRuleCommandHandler(
    IReadActivityRuleRepository readActivityRuleRepository) : ICommandHandler<CreateActivitiesFromActivityRuleCommand>
{
    public async Task HandleAsync(CreateActivitiesFromActivityRuleCommand command, CancellationToken cancellationToken)
    {
        var activityRules = await readActivityRuleRepository
            .GetByIdsAsync(
                command.ActivityRuleIds,
                command.AccountId,
                cancellationToken);

        if (activityRules.Count != command.ActivityRuleIds.Count)
        {
            throw new InvalidArgumentException("CreateActivityFromActivityRule.InvalidNumberOfActivityRules");
        }
        
        
    }
}