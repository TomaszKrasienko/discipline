using System.Linq.Expressions;
using discipline.hangfire.shared.abstractions.DAL;
using Microsoft.EntityFrameworkCore;

namespace discipline.hangfire.infrastructure.DAL.Repositories;

internal sealed class WriteRepository<TEntity, TIdentifier>(
    Context context) : IWriteRepository<TEntity, TIdentifier> where TEntity : class
{
    public async Task AddAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        context.Set<TEntity>().Add(entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddRangeAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        context.Set<TEntity>().AddRange(entities);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        context.Set<TEntity>().Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => context.SaveChangesAsync(cancellationToken);

    public async Task<TEntity?> GetByIdAsync(
        TIdentifier id,
        CancellationToken cancellationToken = default)
        => await context.Set<TEntity>().FindAsync(keyValues:[id], cancellationToken:cancellationToken);

    public Task<TEntity?> SingleOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
        => context.Set<TEntity>().SingleOrDefaultAsync(predicate, cancellationToken);

    public Task<bool> DoesExistAsync(
        Expression<Func<TEntity,bool>> predicate,
        CancellationToken cancellationToken = default)
        => context.Set<TEntity>().AnyAsync(predicate, cancellationToken);
}