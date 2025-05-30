using discipline.centre.activityrules.application.ActivityRules.Events;
using discipline.centre.activityrules.domain.Repositories;
using discipline.centre.activityrules.domain.Specifications;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Events;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.application.ActivityRules.Commands;

public sealed record UpdateActivityRuleCommand(UserId UserId, 
    ActivityRuleId Id, 
    ActivityRuleDetailsSpecification Details,
    ActivityRuleModeSpecification Mode) : ICommand;
internal sealed class UpdateActivityRuleCommandHandler(
    IReadWriteActivityRuleRepository readWriteActivityRuleRepository,
    IEventProcessor eventProcessor) : ICommandHandler<UpdateActivityRuleCommand>
{
    public async Task HandleAsync(UpdateActivityRuleCommand command, CancellationToken cancellationToken = default)
    {
        var activityRule = await readWriteActivityRuleRepository.GetByIdAsync(command.Id, command.UserId, cancellationToken);

        if (activityRule is null)
        {
            throw new NotFoundException("UpdateActivityRule.ActivityRuleNotFound", command.Id.ToString());
        }
        
        var isTitleExists = await readWriteActivityRuleRepository.ExistsAsync(command.Details.Title, command.UserId, cancellationToken);
        if (isTitleExists && activityRule.Details.Title != command.Details.Title)
        {
            throw new NotUniqueException("UpdateActivityRule.NotUniqueTitle", command.Details.Title);
        }
        
        activityRule.Edit(command.Details, command.Mode);
        
        await readWriteActivityRuleRepository.UpdateAsync(activityRule, cancellationToken);
        
        await eventProcessor.PublishAsync(activityRule.DomainEvents.Select(x
            => x.MapAsIntegrationEvent()).ToArray());
    }
}