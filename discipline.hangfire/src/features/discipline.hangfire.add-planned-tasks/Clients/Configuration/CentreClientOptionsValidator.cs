using Microsoft.Extensions.Options;

namespace discipline.hangfire.add_planned_tasks.Clients.Configuration;

internal sealed class CentreClientOptionsValidator : IValidateOptions<CentreClientOptions>
{
    public ValidateOptionsResult Validate(string? name, CentreClientOptions options)
        => string.IsNullOrEmpty(options.Url) 
            ? ValidateOptionsResult.Fail("Centre URL is required") 
            : ValidateOptionsResult.Success;
}