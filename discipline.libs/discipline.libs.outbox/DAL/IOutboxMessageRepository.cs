using System.Data;
using discipline.libs.outbox.Models;

namespace discipline.libs.outbox.DAL;

internal interface IOutboxMessageRepository
{
    Task AddAsync(OutboxMessage message, CancellationToken cancellationToken);
    
    Task<OutboxMessage> GetUnsentMessagesAsync(
        IDbTransaction? transaction = null,
        CancellationToken? cancellationToken = null);
    
    IDbTransaction BeginTransaction();
    
}