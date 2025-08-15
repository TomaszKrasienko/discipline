using FluentValidation;

namespace discipline.centre.users.application.Accounts.DTOs.Requests.Validators;

//TODO: Unit tests
public sealed class RefreshRequestDtoValidator : AbstractValidator<RefreshRequestDto>
{
    public RefreshRequestDtoValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithErrorCode("Validation.RefreshToken.Empty");
        
        RuleFor(x => x.AccountId)
            .NotEmpty()
            .WithErrorCode("Validation.AccountId.Empty")
            .Must(x => Ulid.TryParse(x, out _))
            .WithErrorCode("Validation.AccountId.InvalidFormat");
    }
}