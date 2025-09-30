using discipline.libs.outbox.Models;
using Microsoft.EntityFrameworkCore;

namespace discipline.libs.outbox.DAL;

internal sealed class OutboxDbContext(DbContextOptions<OutboxDbContext> options) : DbContext(options)
{
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("outbox");
        
        modelBuilder
            .Entity<OutboxMessage>()
            .HasKey(x => x.MessageId);

        modelBuilder
            .Entity<OutboxMessage>()
            .Property(x => x.MessageId)
            .IsRequired();
        
        modelBuilder
            .Entity<OutboxMessage>()
            .Property(x => x.JsonContent)
            .IsRequired();
        
        modelBuilder
            .Entity<OutboxMessage>()
            .Property(x => x.MessageType)
            .IsRequired();
        
        modelBuilder
            .Entity<OutboxMessage>()
            .Property(x => x.CreatedAt)
            .IsRequired();
        
        modelBuilder
            .Entity<OutboxMessage>()
            .Property(x => x.SentAt);
        
        modelBuilder
            .Entity<OutboxMessage>()
            .Property(x => x.RetryCount);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<Ulid>()
            .HaveConversion<UlidTypeConverter>();
    }
}