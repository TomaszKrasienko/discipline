namespace discipline.centre.users.application.Users.DTOs.Responses;

public sealed record SubscriptionOrderIntervalResponseDto(
    DateOnly StartDate,
    DateOnly? PlannedFinishDate,
    DateOnly? FinishDate);