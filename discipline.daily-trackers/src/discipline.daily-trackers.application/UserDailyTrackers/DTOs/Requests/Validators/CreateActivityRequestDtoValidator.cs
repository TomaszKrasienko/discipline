using FluentValidation;

namespace discipline.daily_trackers.application.UserDailyTrackers.DTOs.Requests.Validators;

//TODO: Unit tests
internal sealed class CreateActivityRequestDtoValidator : AbstractValidator<CreateActivityRequestDto>
{
    public CreateActivityRequestDtoValidator()
    {
        RuleFor(x => x.Day)
            .NotEqual(default(DateOnly))
            .WithErrorCode("ActivityDetails.Validation.Day.Required");
        
        RuleFor(x => x.ActivityDetails)
            .SetValidator(new ActivityDetailsRequestDtoValidator());
    }
}