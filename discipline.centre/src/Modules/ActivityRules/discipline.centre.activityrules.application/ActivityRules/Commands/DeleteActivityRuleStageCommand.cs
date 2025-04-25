using discipline.centre.activityrules.domain.Repositories;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.application.ActivityRules.Commands;

public sealed record DeleteActivityRuleStageCommand(UserId UserId,
    ActivityRuleId ActivityRuleId, 
    StageId StageId) : ICommand;

internal sealed class DeletActivityRuleStageCommandHandler(
    IReadWriteActivityRuleRepository readWriteActivityRuleRepository) : ICommandHandler<DeleteActivityRuleStageCommand>
{
    public async Task HandleAsync(DeleteActivityRuleStageCommand command, CancellationToken cancellationToken)
    {
        var activityRule = await readWriteActivityRuleRepository.GetByIdAsync(command.ActivityRuleId, 
            command.UserId, cancellationToken);

        if (activityRule is null)
        {
            return;
        }
        
        activityRule
    }
}