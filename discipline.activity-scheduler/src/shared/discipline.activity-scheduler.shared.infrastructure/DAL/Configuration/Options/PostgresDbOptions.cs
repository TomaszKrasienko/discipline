namespace discipline.activity_scheduler.shared.infrastructure.DAL.Configuration.Options;

internal sealed record PostgresDbOptions
{
    public required string ConnectionString { get; init; }
}