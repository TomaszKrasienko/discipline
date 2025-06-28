namespace discipline.hangfire.infrastructure.Postgres.Configuration;

public sealed record PostgresBusinessOptions
{
    public required string ConnectionString { get; init; }
}