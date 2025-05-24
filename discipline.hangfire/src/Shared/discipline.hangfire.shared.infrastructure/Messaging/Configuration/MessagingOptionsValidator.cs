using Microsoft.Extensions.Options;

namespace discipline.hangfire.infrastructure.Messaging.Configuration;

public class MessagingOptionsValidator : IValidateOptions<MessagingOptions>
{
    public ValidateOptionsResult Validate(string? name, MessagingOptions options)
    {
        return ValidateOptionsResult.Success;
    }
}