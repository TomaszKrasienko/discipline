using System.Linq.Expressions;
using discipline.centre.shared.infrastructure.DAL.Collections.Abstractions;
using MongoDB.Driver;

namespace discipline.centre.shared.infrastructure.DAL;

public abstract class BaseRepository<TDocument>(
    MongoCollectionContext context) where TDocument : class, IDocument
{
    protected virtual Task<bool> AnyAsync(Expression<Func<TDocument, bool>> expression, CancellationToken cancellationToken = default)
        => context
            .GetCollection<TDocument>()
            .Find(expression.ToFilterDefinition())
            .AnyAsync(cancellationToken);
    
    protected virtual async Task<TDocument?> GetAsync(Expression<Func<TDocument, bool>> expression, CancellationToken cancellationToken = default)
        => await context.GetCollection<TDocument>()
            .Find(expression.ToFilterDefinition())
            .SingleOrDefaultAsync(cancellationToken);
    
    protected virtual async Task<List<TDocument>> SearchAsync(
        Expression<Func<TDocument, bool>> expression,
        CancellationToken cancellationToken = default)
        => await context.GetCollection<TDocument>()
            .Find(expression.ToFilterDefinition())
            .ToListAsync(cancellationToken);

    protected virtual async Task AddAsync(TDocument document, CancellationToken cancellationToken = default)
    {
        await context
            .GetCollection<TDocument>()
            .InsertOneAsync(document, cancellationToken:cancellationToken);
    }
}