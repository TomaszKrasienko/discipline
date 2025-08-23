namespace discipline.centre.users.application.Subscriptions.DTOs;

public sealed record SubscriptionResponseDto(
    string Id,
    string Type,
    bool RequirePayment,
    bool HasExpiryDate,
    int? AllowedNumberOfDailyTasks,
    int? AllowedNumberOfRules,
    IReadOnlyCollection<SubscriptionPriceResponseDto> Prices);