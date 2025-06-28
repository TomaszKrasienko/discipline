namespace discipline.centre.users.application.Users.DTOs.Endpoints;

public sealed record RefreshRequestDto(
    string RefreshToken,
    string UserId);