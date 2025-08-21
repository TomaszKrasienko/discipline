namespace discipline.centre.users.application.Users.DTOs.Responses;

public sealed record SubscriptionOrderPaymentResponseDto(
    DateTimeOffset CreatedAt,
    decimal Value);