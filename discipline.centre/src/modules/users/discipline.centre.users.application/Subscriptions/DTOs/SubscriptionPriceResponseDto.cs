namespace discipline.centre.users.application.Subscriptions.DTOs;

public sealed record SubscriptionPriceResponseDto(
    decimal PricePerMonth,
    decimal PricePerYear,
    string Currency);