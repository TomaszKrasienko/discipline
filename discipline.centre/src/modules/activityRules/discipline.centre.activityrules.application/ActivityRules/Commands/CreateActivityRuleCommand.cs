using discipline.centre.activityrules.application.ActivityRules.Events;
using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.domain.Repositories;
using discipline.centre.activityrules.domain.Specifications;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Events;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.application.ActivityRules.Commands;

public sealed record CreateActivityRuleCommand(AccountId AccountId, 
    ActivityRuleId Id, 
    ActivityRuleDetailsSpecification Details,
    ActivityRuleModeSpecification Mode) : ICommand;

internal sealed class CreateActivityRuleCommandHandler(
    IReadWriteActivityRuleRepository readWriteActivityRuleRepository,
    IEventProcessor eventProcessor) : ICommandHandler<CreateActivityRuleCommand>
{
    public async Task HandleAsync(CreateActivityRuleCommand command, CancellationToken cancellationToken = default)
    {
        var isExists = await readWriteActivityRuleRepository.ExistsAsync(command.Details.Title, command.AccountId, cancellationToken);
        if (isExists)
        {
            throw new NotUniqueException("CreateActivityRule.NotUniqueTitle",command.Details.Title);
        }

        var activityRule = ActivityRule.Create(command.Id, command.AccountId, command.Details,
            command.Mode);
        
        await readWriteActivityRuleRepository.AddAsync(activityRule, cancellationToken);
        await eventProcessor.PublishAsync(activityRule.DomainEvents.Select(x
            => x.MapAsIntegrationEvent()).ToArray());
    }
}