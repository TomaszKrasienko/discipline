namespace discipline.hangfire.infrastructure.Auth.Configuration;

internal sealed record JwtOptions
{
    public required string PrivateCertPath { get; init; }
    public required string PrivateCertPassword { get; init; }
    public required TimeSpan TokenExpiry { get; init; }
    public required string Issuer { get; init; } 
    public required string Audience { get; init; }
}