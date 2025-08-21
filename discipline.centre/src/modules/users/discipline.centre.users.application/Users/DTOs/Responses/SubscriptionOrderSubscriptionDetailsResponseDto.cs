namespace discipline.centre.users.application.Users.DTOs.Responses;

public sealed record SubscriptionOrderSubscriptionDetailsResponseDto(
    string Type,
    string? ValidityPeriod,
    bool RequirePayment);