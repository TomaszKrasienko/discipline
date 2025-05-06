using Microsoft.Extensions.Options;

namespace discipline.hangfire.infrastructure.Configuration.Options;

internal sealed class AppOptionsValidator : IValidateOptions<AppOptions>
{
    public ValidateOptionsResult Validate(string? name, AppOptions options)
        => string.IsNullOrWhiteSpace(options.Name) 
            ? ValidateOptionsResult.Fail("App name is required") 
            : ValidateOptionsResult.Success;
}