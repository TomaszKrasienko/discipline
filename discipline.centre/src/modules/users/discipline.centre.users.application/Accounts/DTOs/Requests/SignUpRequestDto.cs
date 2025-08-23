namespace discipline.centre.users.application.Accounts.DTOs.Requests;

public sealed record SignUpRequestDto(
    string Email,
    string Password,
    string SubscriptionId,
    string? Period, //TODO: Period should be nullable if subscription not require payment
    string FirstName,
    string LastName,
    decimal? PaymentValue);