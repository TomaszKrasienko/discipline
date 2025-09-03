using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;

namespace discipline.daily_trackers.domain.SharedKernel.Aggregate;

public abstract class AggregateRoot<TIdentifier, TValue>(TIdentifier id) : Entity<TIdentifier, TValue>(id), IAggregateRoot 
    where TIdentifier : struct, ITypeId<TIdentifier, TValue>
    where TValue : struct
{
    private readonly List<DomainEvent> _domainEvents = [];
    
    public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents;

    protected void AddDomainEvent(DomainEvent @event)
        => _domainEvents.Add(@event);

    public void ClearDomainEvents()
        => _domainEvents.Clear();
}



