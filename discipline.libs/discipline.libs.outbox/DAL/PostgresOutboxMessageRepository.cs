using System.Data;
using discipline.libs.outbox.Models;

namespace discipline.libs.outbox.DAL;

internal sealed class PostgresOutboxMessageRepository : IOutboxMessageRepository
{
    public Task AddAsync(OutboxMessage message, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<OutboxMessage> GetUnsentMessagesAsync(IDbTransaction? transaction = null, CancellationToken? cancellationToken = null)
    {
        throw new NotImplementedException();
    }

    public IDbTransaction BeginTransaction()
    {
        throw new NotImplementedException();
    }
}