namespace discipline.centre.users.application.Accounts.DTOs.Requests;

public sealed record SignUpRequestDto(
    string Email,
    string Password,
    string SubscriptionId,
    string Period,
    string FirstName,
    string LastName,
    decimal? PaymentValue);