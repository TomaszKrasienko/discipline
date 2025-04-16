using FluentValidation;

namespace discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.ActivityRules.Validators;

internal sealed class ActivityRuleModeRequestDtoValidator : AbstractValidator<ActivityRuleModeRequestDto>
{
    public ActivityRuleModeRequestDtoValidator()
    {
        RuleFor(x => x.Mode)
            .NotNull()
            .NotEmpty()
            .WithMessage("Mode cannot be null or empty.");
    }
}