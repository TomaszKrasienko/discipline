using System.Data;
using discipline.hangfire.infrastructure.Postgres.Configuration;
using discipline.hangfire.shared.abstractions.DataAccess;
using Microsoft.Extensions.Options;
using Npgsql;

namespace discipline.hangfire.infrastructure.Postgres;

internal sealed class PostgreSqlDbContext(IOptions<PostgresBusinessOptions> options
    ) : IDbContext
{
    private readonly string _connectionString = options.Value.ConnectionString;
    private IDbConnection? _connection;
    
    public IDbConnection GetConnection()
    {
        _connection = new NpgsqlConnection(_connectionString);
        _connection!.Open();
        return _connection;
    }

    public void Dispose()
        => _connection?.Dispose();
}