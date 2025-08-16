namespace discipline.centre.users.infrastructure.Auth.Configuration.Options;

internal sealed record JwtOptions
{
    public KeyPublishingOptions KeyPublishing { get; init; } = new ();
}