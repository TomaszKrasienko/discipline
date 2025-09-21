using Microsoft.Extensions.Options;

namespace discipline.libs.mongo_db.Configuration.Options;

internal sealed class MongoDbOptionsValidator : IValidateOptions<MongoDbOptions>
{
    public ValidateOptionsResult Validate(string? name, MongoDbOptions options)
    {
        if (string.IsNullOrWhiteSpace(options?.ConnectionString))
        {
            return ValidateOptionsResult.Fail("Mongo DB connection string can not be null or empty");
        }
        return ValidateOptionsResult.Success;
    }
}