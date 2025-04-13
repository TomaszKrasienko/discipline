using FluentValidation;

namespace discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.Create.Validators;

internal sealed class CreateActivityRuleRequestDtoValidator : AbstractValidator<CreateActivityRuleRequestDto>
{
    public CreateActivityRuleRequestDtoValidator()
    {
        RuleFor(x => x.Details)
            .NotNull()
            .WithMessage("Details cannot be null.");

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        When(x => x.Details is not null, () =>
        {
            RuleFor(x => x.Details)
                .SetValidator(new CreateActivityRuleDetailsRequestDtoValidator());
        });
        
        RuleFor(x => x.Mode)
            .NotNull()
            .WithMessage("Mode cannot be null.");

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        When(x => x.Mode is not null, () =>
        {
            RuleFor(x => x.Mode)
                .SetValidator(new CreateActivityRuleModeRequestDtoValidator());
        });
    }
}