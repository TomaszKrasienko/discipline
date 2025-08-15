namespace discipline.centre.users.application.Accounts.DTOs.Requests;

public sealed record RefreshRequestDto(string RefreshToken, string AccountId);