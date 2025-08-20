using FluentValidation;

namespace discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.ActivityRules.Validators;

public sealed class ActivityRuleRequestDtoValidator : AbstractValidator<ActivityRuleRequestDto>
{
    public ActivityRuleRequestDtoValidator()
    {
        RuleFor(x => x.Details)
            .NotNull()
            .WithErrorCode("ActivityRule.Validation.Details.Null");

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        When(x => x.Details is not null, () =>
        {
            RuleFor(x => x.Details)
                .SetValidator(new ActivityRuleDetailsRequestDtoValidator());
        });
        
        RuleFor(x => x.Mode)
            .NotNull()
            .WithErrorCode("ActivityRule.Validation.Mode.Null");

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        When(x => x.Mode is not null, () =>
        {
            RuleFor(x => x.Mode)
                .SetValidator(new ActivityRuleModeRequestDtoValidator());
        });
    }
}