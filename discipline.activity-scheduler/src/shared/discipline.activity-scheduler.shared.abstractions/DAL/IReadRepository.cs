using System.Linq.Expressions;
using discipline.activity_scheduler.shared.abstractions.ViewModels.Abstractions;

namespace discipline.activity_scheduler.shared.abstractions.DAL;

public interface IReadRepository<TViewModel, in TIdentifier> where TViewModel : class, IViewModel
{
    Task<TViewModel?> GetByIdAsync(
        TIdentifier id,
        CancellationToken cancellationToken = default);
    
    Task<IReadOnlyList<TViewModel>> GetAllAsync(
        CancellationToken cancellationToken = default);
    
    Task<IReadOnlyList<TViewModel>> SearchAsync(
        Expression<Func<TViewModel, bool>> predicate,
        CancellationToken cancellationToken = default);
}