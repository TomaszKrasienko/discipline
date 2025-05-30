using FluentValidation;

namespace discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.ActivityRules.Validators;

public sealed class ActivityRuleRequestDtoValidator : AbstractValidator<ActivityRuleRequestDto>
{
    public ActivityRuleRequestDtoValidator()
    {
        RuleFor(x => x.Details)
            .NotNull()
            .WithMessage("Validation.NullDetails");

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        When(x => x.Details is not null, () =>
        {
            RuleFor(x => x.Details)
                .SetValidator(new ActivityRuleDetailsRequestDtoValidator());
        });
        
        RuleFor(x => x.Mode)
            .NotNull()
            .WithMessage("Validation.NullMode");

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        When(x => x.Mode is not null, () =>
        {
            RuleFor(x => x.Mode)
                .SetValidator(new ActivityRuleModeRequestDtoValidator());
        });
    }
}