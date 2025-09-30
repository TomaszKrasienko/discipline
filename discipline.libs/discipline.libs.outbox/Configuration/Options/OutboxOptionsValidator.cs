using Microsoft.Extensions.Options;

namespace discipline.libs.outbox.Configuration.Options;

internal sealed class OutboxOptionsValidator : IValidateOptions<OutboxOptions>
{
    public ValidateOptionsResult Validate(
        string? name,
        OutboxOptions options)
    {
        if (options.IsEnabled
            && string.IsNullOrWhiteSpace(options.ConnectionString))
        {
            return ValidateOptionsResult.Fail(
                "Invalid Outbox connection string");
        }
        
        return ValidateOptionsResult.Success;
    }
}