namespace discipline.centre.users.application.Users.DTOs.Responses;

public sealed record UserResponseDto(
    string UserId,
    string AccountId,
    string Email,
    string FirstName,
    string LastName,
    SubscriptionOrderResponseDto ActiveSubscriptionOrder);