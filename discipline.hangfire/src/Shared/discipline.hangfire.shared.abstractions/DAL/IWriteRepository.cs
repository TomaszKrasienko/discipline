using System.Linq.Expressions;
using discipline.hangfire.shared.abstractions.ViewModels.Abstractions;

namespace discipline.hangfire.shared.abstractions.DAL;

public interface IWriteRepository<TEntity, TIdentifier>  where TEntity : class
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<TEntity?> GetByIdAsync(TIdentifier id, CancellationToken cancellationToken = default);
    Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<bool> DoesExistAsync(Expression<Func<TEntity,bool>> predicate, CancellationToken cancellationToken = default);
}