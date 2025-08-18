using FluentValidation;

namespace discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.ActivityRules.Validators;

public sealed class ActivityRuleDetailsRequestDtoValidator : AbstractValidator<ActivityRuleDetailsRequestDto>
{
    public ActivityRuleDetailsRequestDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotNull()
            .NotEmpty()
            .WithErrorCode("Validation.Details.Title.Empty")
            .MaximumLength(30)
            .WithMessage("Validation.Details.Title.TooLong");
    }
}