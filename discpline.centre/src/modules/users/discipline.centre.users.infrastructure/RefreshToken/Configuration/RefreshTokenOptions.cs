namespace discipline.centre.users.infrastructure.Accounts.RefreshToken.Configuration;

internal sealed class RefreshTokenOptions
{
    public int Length { get; init; }
    public TimeSpan Expiry { get; init; }
}