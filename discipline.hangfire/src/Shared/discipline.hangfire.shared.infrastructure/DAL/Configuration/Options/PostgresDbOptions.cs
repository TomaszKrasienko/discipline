namespace discipline.hangfire.infrastructure.DAL.Configuration.Options;

internal sealed record PostgresDbOptions
{
    public required string ConnectionString { get; init; }
}