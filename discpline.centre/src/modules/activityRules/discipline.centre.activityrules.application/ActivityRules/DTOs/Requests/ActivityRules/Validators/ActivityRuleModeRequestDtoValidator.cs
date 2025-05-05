using FluentValidation;

namespace discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.ActivityRules.Validators;

public sealed class ActivityRuleModeRequestDtoValidator : AbstractValidator<ActivityRuleModeRequestDto>
{
    public ActivityRuleModeRequestDtoValidator()
    {
        RuleFor(x => x.Mode)
            .NotNull()
            .NotEmpty()
            .WithMessage("Validation.EmptyActivityRuleMode");

        When(x => x.Days is not null, () =>
        {
            RuleFor(field => field.Days)
                .Must(days => !days!.Any(d => d > (int)DayOfWeek.Saturday))
                .WithMessage("Validation.ActivityRuleDaysOutOfRange");
        });
    }
}