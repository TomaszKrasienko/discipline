namespace discipline.hangfire.server.Hangfire;

internal sealed record PostgresHangfireOptions
{
    public required string ConnectionString { get; init; }
}