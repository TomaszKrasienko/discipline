using FluentValidation;

namespace discipline.daily_trackers.application.UserDailyTrackers.DTOs.Requests.Validators;

//TODO: Unit tests
internal sealed class ActivityDetailsRequestDtoValidator : AbstractValidator<ActivityDetailsRequestDto>
{
    public ActivityDetailsRequestDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithErrorCode("ActivityDetails.Validation.Title.Required")
            .MaximumLength(30)
            .WithErrorCode("ActivityDetails.Validation.Title.TooLong");
    }
}