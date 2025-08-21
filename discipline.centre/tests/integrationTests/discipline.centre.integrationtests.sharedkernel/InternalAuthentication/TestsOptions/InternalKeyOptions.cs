namespace discipline.centre.integrationtests.sharedkernel.InternalAuthentication.TestsOptions;

internal sealed record InternalKeyOptions
{
    public string PrivateCertPassword { get; init; } = string.Empty;
    public string PrivateCertPath { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
}