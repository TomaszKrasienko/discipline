using Microsoft.Extensions.Options;

namespace discipline.libs.auth.Configuration.Options;

internal sealed class AuthOptionsValidator : IValidateOptions<AuthOptions>
{
    public ValidateOptionsResult Validate(string? name, AuthOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.Audience))
        {
            return ValidateOptionsResult.Fail($"{nameof(options.Audience)} is required");
        }

        if (string.IsNullOrWhiteSpace(options.Issuer))
        {
            return ValidateOptionsResult.Fail($"{nameof(options.Issuer)} is required");
        }
        
        return ValidateOptionsResult.Success;
    }
}