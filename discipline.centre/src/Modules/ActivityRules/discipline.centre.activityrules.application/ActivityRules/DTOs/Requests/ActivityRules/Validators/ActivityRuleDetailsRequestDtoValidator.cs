using FluentValidation;

namespace discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.ActivityRules.Validators;

internal sealed class ActivityRuleDetailsRequestDtoValidator : AbstractValidator<ActivityRuleDetailsRequestDto>
{
    public ActivityRuleDetailsRequestDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotNull()
            .NotEmpty()
            .WithMessage("Title cannot be null or empty.")
            .MaximumLength(30)
            .WithMessage("Title cannot be longer than 30 characters.");
    }
}