using FluentValidation;

namespace discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.Create.Validators;

internal sealed class CreateActivityRuleModeRequestDtoValidator : AbstractValidator<CreateActivityRuleModeRequestDto>
{
    public CreateActivityRuleModeRequestDtoValidator()
    {
        RuleFor(x => x.Mode)
            .NotNull()
            .NotEmpty()
            .WithMessage("Mode cannot be null or empty.");
    }
}