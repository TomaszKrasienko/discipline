using discipline.centre.shared.abstractions.UnitOfWork;
using discipline.centre.shared.infrastructure.DAL.Collections.Abstractions;
using MongoDB.Driver;

namespace discipline.centre.shared.infrastructure.DAL;

internal sealed class MongoUnitOfWork(
    IMongoClient client) : IUnitOfWork, IDisposable
{
    private IClientSessionHandle? _session;
    
    public async Task StartTransactionAsync(CancellationToken cancellationToken = default)
    {
        _session = await client.StartSessionAsync(
            cancellationToken: cancellationToken);
        _session.StartTransaction();
    }

    public Task? CommitTransactionAsync(CancellationToken cancellationToken)
        => _session?.CommitTransactionAsync(cancellationToken);

    public Task? RollbackTransactionAsync(CancellationToken cancellationToken)
        => _session?.AbortTransactionAsync(cancellationToken);

    public void Dispose()
        => _session?.Dispose();
}