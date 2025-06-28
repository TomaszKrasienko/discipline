using Microsoft.Extensions.Options;

namespace discipline.hangfire.infrastructure.Logging.Configuration.Options;

internal sealed class JaegerOptionsValidator : IValidateOptions<JaegerOptions>
{
    public ValidateOptionsResult Validate(string? name, JaegerOptions options)
        => string.IsNullOrWhiteSpace(options.Endpoint) 
            ? ValidateOptionsResult.Fail("Host is required") 
            : ValidateOptionsResult.Success;
}