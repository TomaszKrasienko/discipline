using Microsoft.Extensions.Options;

namespace discipline.hangfire.infrastructure.DAL.Configuration.Options;

internal sealed class PostgresDbOptionsValidator : IValidateOptions<PostgresDbOptions>
{
    public ValidateOptionsResult Validate(string? name, PostgresDbOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.ConnectionString))
        {
            return ValidateOptionsResult.Fail("Postgres DB connection string can not be null or empty");
        }
        return ValidateOptionsResult.Success;
    }
}