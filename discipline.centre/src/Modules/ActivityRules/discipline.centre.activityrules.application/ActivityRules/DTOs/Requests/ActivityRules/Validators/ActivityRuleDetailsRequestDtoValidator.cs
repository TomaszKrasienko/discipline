using FluentValidation;

namespace discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.ActivityRules.Validators;

public sealed class ActivityRuleDetailsRequestDtoValidator : AbstractValidator<ActivityRuleDetailsRequestDto>
{
    public ActivityRuleDetailsRequestDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotNull()
            .NotEmpty()
            .WithMessage("Validation.EmptyActivityRuleTitle")
            .MaximumLength(30)
            .WithMessage("Validation.ActivityRuleTitleTooLong");
    }
}