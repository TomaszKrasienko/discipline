using discipline.centre.activityrules.domain.Repositories;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using FluentValidation;

namespace discipline.centre.activityrules.application.ActivityRules.Commands;

public sealed record CreateStageForActivityRuleCommand(UserId UserId,
    ActivityRuleId ActivityRuleId,
    StageId StageId,
    string Title, 
    int? Index) : ICommand;

internal sealed class CreateStageForActivityRuleCommandValidator : AbstractValidator<CreateStageForActivityRuleCommand>
{
    public CreateStageForActivityRuleCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotNull()
            .NotEmpty()
            .WithMessage("Activity rule stage \"Title\" cannot be null or empty.")
            .MaximumLength(20)
            .WithMessage("Activity rule stage \"Title\" cannot be longer than 20 characters.");

        When(x => x.Index is not null, () =>
        {
            RuleFor(x => x.Index)
                .GreaterThan(0)
                .WithMessage("Activity rule \"Index\" must be greater than zero.");
        });
    }
}

internal sealed class CreateStageForActivityRuleCommandHandler(
    IReadWriteActivityRuleRepository readWriteActivityRuleRepository) : ICommandHandler<CreateStageForActivityRuleCommand>
{
    public async Task HandleAsync(CreateStageForActivityRuleCommand command, CancellationToken cancellationToken)
    {
        var activityRule = await readWriteActivityRuleRepository
            .GetByIdAsync(command.ActivityRuleId, command.UserId, cancellationToken);

        if (activityRule is null)
        {
            throw new NotFoundException("CreateStageForActivityRule.ActivityRuleNotFound", nameof(activityRule), command.ActivityRuleId.ToString());
        }
    }
}