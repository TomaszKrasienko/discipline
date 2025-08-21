namespace discipline.centre.users.application.Users.DTOs.Responses;

public sealed record SubscriptionOrderResponseDto(
    SubscriptionOrderIntervalResponseDto Interval,
    SubscriptionOrderSubscriptionDetailsResponseDto SubscriptionDetails,
    SubscriptionOrderPaymentResponseDto? Payment,
    string SubscriptionId);