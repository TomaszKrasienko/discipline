namespace discipline.centre.users.infrastructure.RefreshToken.Configuration;

internal sealed class RefreshTokenOptions
{
    public int Length { get; init; }
    public TimeSpan Expiry { get; init; }
}