namespace discipline.libs.mongo_db.Configuration.Options;

internal sealed record MongoDbOptions
{
    public string? ConnectionString { get; init; }
}