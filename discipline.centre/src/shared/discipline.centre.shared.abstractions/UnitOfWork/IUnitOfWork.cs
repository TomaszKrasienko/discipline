namespace discipline.centre.shared.abstractions.UnitOfWork;

public interface IUnitOfWork
{
    Task StartTransactionAsync(CancellationToken cancellationToken = default);
    Task? CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task? RollbackTransactionAsync(CancellationToken cancellationToken = default);
}