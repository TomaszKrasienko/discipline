using FluentValidation;

namespace discipline.centre.users.application.Accounts.DTOs.Requests.Validators;

//TODO: Unit tests
public sealed class SignInRequestDtoValidator : AbstractValidator<SignInRequestDto>
{
    public SignInRequestDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithErrorCode("Validation.Email.Empty")
            .EmailAddress()
            .WithErrorCode("Validation.Email.InvalidFormat");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithErrorCode("Validation.Password.Empty");
    }
}