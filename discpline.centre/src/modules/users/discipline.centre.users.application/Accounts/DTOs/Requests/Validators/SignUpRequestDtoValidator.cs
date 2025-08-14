using FluentValidation;

namespace discipline.centre.users.application.Accounts.DTOs.Requests.Validators;

//TODO: Unit tests
public sealed class SignUpRequestDtoValidator : AbstractValidator<SignUpRequestDto>
{
    public SignUpRequestDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithErrorCode("Validation.Email.Empty")
            .EmailAddress()
            .WithErrorCode("Validation.Email.InvalidFormat");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithErrorCode("Validation.Password.Empty");
        
        RuleFor(x => x.SubscriptionId)
            .Must(x => Ulid.TryParse(x, out _))
            .WithErrorCode("Validation.SubscriptionId.InvalidFormat");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithErrorCode("Validation.FirstName.Empty")
            .MaximumLength(100)
            .WithErrorCode("Validation.FirstName.TooLong")
            .MinimumLength(2)
            .WithErrorCode("Validation.FirstName.TooShort");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithErrorCode("Validation.LastName.Empty")
            .MaximumLength(100)
            .WithErrorCode("Validation.LastName.TooLong")
            .MinimumLength(2)
            .WithErrorCode("Validation.LastName.TooShort");
    }
}