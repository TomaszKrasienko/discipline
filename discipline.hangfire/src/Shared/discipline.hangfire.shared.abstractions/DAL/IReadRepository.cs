using System.Linq.Expressions;
using discipline.hangfire.shared.abstractions.ViewModels.Abstractions;

namespace discipline.hangfire.shared.abstractions.DAL;

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