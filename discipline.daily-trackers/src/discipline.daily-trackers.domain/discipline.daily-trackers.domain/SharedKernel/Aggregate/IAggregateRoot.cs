namespace discipline.daily_trackers.domain.SharedKernel.Aggregate;

public interface IAggregateRoot
{
    public IReadOnlyList<DomainEvent> DomainEvents { get; }
}