using FluentValidation;

namespace discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.Create.Validators;

internal sealed class CreateActivityRuleDetailsRequestDtoValidator : AbstractValidator<CreateActivityRuleDetailsRequestDto>
{
    public CreateActivityRuleDetailsRequestDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotNull()
            .NotEmpty()
            .WithMessage("Title cannot be null or empty.")
            .MaximumLength(30)
            .WithMessage("Title cannot be longer than 30 characters.");
    }
}