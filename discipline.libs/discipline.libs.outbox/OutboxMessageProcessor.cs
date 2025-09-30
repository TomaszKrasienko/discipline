using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace discipline.libs.outbox;

public sealed class OutboxMessageProcessor(
    ILogger<OutboxMessageProcessor> logger) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Outbox processor started");
        try
        {
            
        }
    }
    
    // var sqlQuery = $"SELECT * FROM outbox.\"OutboxMessages\" WHERE \"ProcessedAt\" IS NULL ORDER BY \"StoredAt\" DESC {(batchSize > 0? $"LIMIT {batchSize}" : "")} FOR UPDATE SKIP LOCKED";


    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}