using System.Linq.Expressions;
using discipline.activity_scheduler.shared.abstractions.DAL;
using discipline.activity_scheduler.shared.abstractions.ViewModels.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace discipline.activity_scheduler.shared.infrastructure.DAL.Repositories;

internal sealed class ReadRepository<TViewModel, TIdentifier>(
    Context context) : IReadRepository<TViewModel, TIdentifier> where TViewModel : class, IViewModel
{
    public async Task<TViewModel?> GetByIdAsync(
        TIdentifier id,
        CancellationToken cancellationToken = default)
        => await context
            .Set<TViewModel>()
            .FindAsync(keyValues:[id], cancellationToken:cancellationToken);

    public async Task<IReadOnlyList<TViewModel>> GetAllAsync(CancellationToken cancellationToken = default)
        => await context
            .Set<TViewModel>()
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<TViewModel>> SearchAsync(
        Expression<Func<TViewModel, bool>> predicate,
        CancellationToken cancellationToken = default)
        => await context.Set<TViewModel>()
            .Where(predicate)
            .ToListAsync(cancellationToken);
}