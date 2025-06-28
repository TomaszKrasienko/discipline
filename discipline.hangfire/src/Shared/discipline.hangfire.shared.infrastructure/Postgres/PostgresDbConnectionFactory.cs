using System.Data;
using discipline.hangfire.infrastructure.Postgres.Configuration;
using discipline.hangfire.shared.abstractions.DataAccess;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;

namespace discipline.hangfire.infrastructure.Postgres;

internal sealed class PostgresDbConnectionFactory(
    ILogger<PostgresDbConnectionFactory> logger,
    IOptions<PostgresBusinessOptions> options)
    : IDbConnectionFactory
{
    private readonly string _connectionString = options.Value.ConnectionString;

    public IDbConnection CreateConnection()
    {
        logger.LogDebug("Creating Postgres connection");
        var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        return connection;
    }
}