using FluentValidation;

namespace discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.Stages.Validators;

public sealed class CreateStageRequestDtoValidator : AbstractValidator<CreateStageRequestDto>
{
    public CreateStageRequestDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotNull()
            .NotEmpty()
            .WithErrorCode("CreateStage.Validation.Title.Empty")
            .MaximumLength(30)
            .WithErrorCode("CreateStage.Validation.Title.TooLong");
    }
}