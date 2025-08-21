using discipline.centre.activityrules.domain.Repositories;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.application.ActivityRules.Commands;

public sealed record DeleteActivityRuleStageCommand(AccountId AccountId,
    ActivityRuleId ActivityRuleId, 
    StageId StageId) : ICommand;

internal sealed class DeleteActivityRuleStageCommandHandler(
    IReadWriteActivityRuleRepository readWriteActivityRuleRepository) : ICommandHandler<DeleteActivityRuleStageCommand>
{
    public async Task HandleAsync(DeleteActivityRuleStageCommand command, CancellationToken cancellationToken)
    {
        var activityRule = await readWriteActivityRuleRepository.GetByIdAsync(
            command.ActivityRuleId, 
            command.AccountId,
            cancellationToken);

        if (activityRule is null)
        {
            return;
        }
        
        var isSuccess = activityRule.RemoveStage(command.StageId);

        if (isSuccess)
        {
            await readWriteActivityRuleRepository.UpdateAsync(activityRule, cancellationToken);
        }
    }
}