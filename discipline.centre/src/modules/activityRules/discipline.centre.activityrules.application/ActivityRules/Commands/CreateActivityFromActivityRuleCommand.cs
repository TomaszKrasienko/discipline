using discipline.centre.activityrules.application.ActivityRules.Commands.External;
using discipline.centre.activityrules.application.ActivityRules.DTOs.Commands;
using discipline.centre.activityrules.domain.Repositories;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.Messaging;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.libs.cqrs.abstractions.Commands;

namespace discipline.centre.activityrules.application.ActivityRules.Commands;

public sealed record CreateActivitiesFromActivityRuleCommand(
    IReadOnlyCollection<ActivityRuleId> ActivityRuleIds,
    AccountId AccountId) : ICommand;
    
internal sealed class CreateActivityFromActivityRuleCommandHandler(
    IReadActivityRuleRepository readActivityRuleRepository,
    ICommandProcessor commandProcessor,
    TimeProvider timeProvider) : ICommandHandler<CreateActivitiesFromActivityRuleCommand>
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

        var externalCommand = new CreateActivityCommand(
            command.AccountId.ToString(),
            DateOnly.FromDateTime(timeProvider.GetUtcNow().DateTime),
            activityRules.Select(activityRule 
                => new CreateActivityDto(
                    activityRule.Id.ToString(),
                    new CreateActivityDetailsDto(activityRule.Details.Title, activityRule.Details.Note),
                    activityRule.Stages.Select(stage 
                        => new CreateActivityStageDto(stage.Title, stage.Index)).ToList())).ToList());
        
        await commandProcessor.PublishAsync(cancellationToken, externalCommand);
    }
}