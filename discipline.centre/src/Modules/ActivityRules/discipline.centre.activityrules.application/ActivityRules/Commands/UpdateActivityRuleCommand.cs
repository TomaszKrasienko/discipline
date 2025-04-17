using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.domain.Repositories;
using discipline.centre.activityrules.domain.Specifications;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using FluentValidation;

namespace discipline.centre.activityrules.application.ActivityRules.Commands;

public sealed record UpdateActivityRuleCommand(UserId UserId, 
    ActivityRuleId Id, 
    ActivityRuleDetailsSpecification Details,
    ActivityRuleModeSpecification Mode) : ICommand;

public sealed class UpdateActivityRuleCommandValidator : AbstractValidator<UpdateActivityRuleCommand>
{
    public UpdateActivityRuleCommandValidator()
    {
        RuleFor(x => x.Details.Title)
            .NotNull()
            .NotEmpty()
            .WithMessage("Activity rule \"Title\" can not be null or empty");
        RuleFor(x => x.Details.Title)
            .MaximumLength(30)
            .WithMessage("Activity rule \"Title\" has invalid length");
        RuleFor(x => x.Mode)
            .NotNull()
            .NotEmpty()
            .WithMessage("Activity rule \"Mode\" can not be null or empty");
    }
}

internal sealed class UpdateActivityRuleCommandHandler(
    IReadWriteActivityRuleRepository readWriteActivityRuleRepository) : ICommandHandler<UpdateActivityRuleCommand>
{
    public async Task HandleAsync(UpdateActivityRuleCommand command, CancellationToken cancellationToken = default)
    {
        var activityRule = await readWriteActivityRuleRepository.GetByIdAsync(command.Id, command.UserId, cancellationToken);

        if (activityRule is null)
        {
            throw new NotFoundException("UpdateActivityRule.ActivityRuleNotFound", nameof(activityRule), command.Id.ToString());
        }
        
        var isTitleExists = await readWriteActivityRuleRepository.ExistsAsync(command.Details.Title, command.UserId, cancellationToken);
        if (isTitleExists && activityRule.Details.Title != command.Details.Title)
        {
            throw new AlreadyRegisteredException("UpdateActivityRule.NotUniqueTitle",
                $"Activity rule with title: {command.Details.Title} already registered");
        }
        
        activityRule.Edit(command.Details, command.Mode);
        await readWriteActivityRuleRepository.UpdateAsync(activityRule, cancellationToken);
    }
}