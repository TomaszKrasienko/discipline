namespace discipline.libs.auth.Configuration.Options;

internal sealed record AuthOptions
{
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required string PublicCertificatePath { get; init; }
}