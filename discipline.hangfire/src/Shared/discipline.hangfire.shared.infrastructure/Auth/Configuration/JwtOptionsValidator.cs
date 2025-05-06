using System.Text;
using Microsoft.Extensions.Options;

namespace discipline.hangfire.infrastructure.Auth.Configuration;

internal sealed class JwtOptionsValidator : IValidateOptions<JwtOptions>
{
    public ValidateOptionsResult Validate(string? name, JwtOptions options)
    {
        StringBuilder errorMessagesBuilder = new();
        List<KeyValuePair<bool, string?>> stringKeyPublishingValidationResults =
        [
            ValidateIfStringIsEmpty(options.PrivateCertPath, nameof(options.PrivateCertPath)),
            ValidateIfStringIsEmpty(options.PrivateCertPassword, nameof(options.PrivateCertPassword)),
            ValidateIfStringIsEmpty(options.Issuer, nameof(options.Issuer)),
            ValidateIfStringIsEmpty(options.Audience, nameof(options.Audience)),
        ];

        if (!stringKeyPublishingValidationResults.TrueForAll(x => x.Key))
        {
            errorMessagesBuilder.Append(
            string.Join(',', stringKeyPublishingValidationResults
                .Where(x => !x.Key)
                .Select(x => x.Value)
                .ToArray()));
        }

        if (options.TokenExpiry == TimeSpan.Zero)
        {
            errorMessagesBuilder.Append("The token expiry cannot be zero, ");
        }
        
        return errorMessagesBuilder.Length == 0 
            ? ValidateOptionsResult.Success 
            : ValidateOptionsResult.Fail(errorMessagesBuilder.ToString());
    }
    
    private static KeyValuePair<bool, string?> ValidateIfStringIsEmpty(string value, string fieldName)
        => string.IsNullOrWhiteSpace(value) 
            ? new KeyValuePair<bool, string?>(false, $"The field {fieldName} cannot be empty") 
            : new KeyValuePair<bool, string?>(true, null);
}