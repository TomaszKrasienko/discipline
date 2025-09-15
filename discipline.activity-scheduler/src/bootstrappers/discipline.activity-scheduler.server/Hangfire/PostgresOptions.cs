namespace discipline.activity_scheduler.server.Hangfire;

internal sealed record PostgresHangfireOptions
{
    public required string ConnectionString { get; init; }
}