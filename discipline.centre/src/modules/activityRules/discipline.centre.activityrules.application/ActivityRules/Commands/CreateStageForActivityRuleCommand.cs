using discipline.centre.activityrules.domain.Repositories;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using FluentValidation;

namespace discipline.centre.activityrules.application.ActivityRules.Commands;

public sealed record CreateStageForActivityRuleCommand(AccountId AccountId,
    ActivityRuleId ActivityRuleId,
    StageId StageId,
    string Title, 
    int? Index) : ICommand;

internal sealed class CreateStageForActivityRuleCommandHandler(
    IReadWriteActivityRuleRepository readWriteActivityRuleRepository) : ICommandHandler<CreateStageForActivityRuleCommand>
{
    public async Task HandleAsync(CreateStageForActivityRuleCommand command, CancellationToken cancellationToken)
    {
        var activityRule = await readWriteActivityRuleRepository
            .GetByIdAsync(command.ActivityRuleId, command.AccountId, cancellationToken);

        if (activityRule is null)
        {
            throw new NotFoundException("CreateStageForActivityRule.ActivityRuleNotFound", command.ActivityRuleId.ToString());
        }

        activityRule.AddStage(command.StageId, command.Title);
        await readWriteActivityRuleRepository.UpdateAsync(activityRule, cancellationToken);
    }
}