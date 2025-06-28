using System.Data;
using discipline.hangfire.shared.abstractions.DataAccess;
using Microsoft.Extensions.Logging;

namespace discipline.hangfire.infrastructure.Postgres;

internal sealed class DbTransactionManager(
    ILogger<DbTransactionManager> logger,
    IDbConnectionFactory dbConnectionFactory) : IDbTransactionManager, IDisposable
{
    private readonly IDbConnection _connection = dbConnectionFactory.CreateConnection();
    private IDbTransaction? _transaction;
    
    public IDbConnection Connection => _connection;

    public IDbTransaction Begin()
    {
        logger.LogDebug("Begging transaction");
        _transaction = _connection.BeginTransaction();
        return _transaction;
    }

    public void Commit()
    {
        logger.LogDebug("Committing transaction");
        _transaction?.Commit();
        _transaction?.Dispose();
    }

    public void Rollback()
    {
        logger.LogDebug("Rolling back transaction");
        _transaction?.Rollback();
        Dispose();
    }
    
    public void Dispose()
    {
        _transaction?.Dispose();
        _connection?.Dispose();
    }
}