using Microsoft.Extensions.Options;

namespace discipline.activity_scheduler.add_planned_tasks.Clients.Configuration.Options;

internal sealed class CentreClientOptionsValidator : IValidateOptions<CentreClientOptions>
{
    public ValidateOptionsResult Validate(string? name, CentreClientOptions options)
        => string.IsNullOrEmpty(options.Url) 
            ? ValidateOptionsResult.Fail("Centre URL is required") 
            : ValidateOptionsResult.Success;
}