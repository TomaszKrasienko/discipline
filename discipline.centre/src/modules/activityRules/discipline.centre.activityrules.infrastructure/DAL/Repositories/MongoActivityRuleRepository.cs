using discipline.centre.activity_rules.infrastructure.DAL.Documents;
using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.domain.Repositories;
using discipline.centre.activityrules.infrastructure.DAL.Documents;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.shared.infrastructure.DAL;
using MongoDB.Driver;

namespace discipline.centre.activity_rules.infrastructure.DAL.Repositories;

internal sealed class MongoActivityRuleRepository(
    ActivityRulesMongoContext context) : BaseRepository<ActivityRuleDocument>(context), IReadWriteActivityRuleRepository 
{
    public async Task<ActivityRule?> GetByIdAsync(ActivityRuleId id, AccountId accountId, CancellationToken cancellationToken = default)
        => (await GetAsync(x 
                => x.Id == id.Value.ToString() && 
                   x.AccountId == accountId.ToString(), 
                cancellationToken))?
            .AsEntity();

    public async Task<IReadOnlyCollection<ActivityRule>> GetByIdsAsync(IReadOnlyCollection<ActivityRuleId> ids,
        AccountId accountId, CancellationToken cancellationToken = default)
        => (await SearchAsync(x =>
                x.AccountId == accountId.ToString() &&
                ids.Select(id => id.ToString()).Contains(x.Id),
                cancellationToken))
            .Select(x => x.AsEntity())
            .ToList();
    
    public Task<bool> ExistsAsync(string title, AccountId accountId, CancellationToken cancellationToken = default)
        => AnyAsync(x 
                => x.Details.Title == title && 
                   x.AccountId == accountId.ToString(),
            cancellationToken);
    
    public Task AddAsync(ActivityRule activityRule, CancellationToken cancellationToken = default)
        => AddAsync(activityRule.ToDocument(), cancellationToken);

    public async Task UpdateAsync(ActivityRule activityRule, CancellationToken cancellationToken = default)
        => await context.GetCollection<ActivityRuleDocument>()
            .FindOneAndReplaceAsync(x 
                    => x.Id == activityRule.Id.ToString(),
                activityRule.ToDocument(),
                null,
                cancellationToken);

    public Task DeleteAsync(ActivityRule activityRule, CancellationToken cancellationToken = default)
        => context.GetCollection<ActivityRuleDocument>()
            .FindOneAndDeleteAsync(x => x.Id == activityRule.Id.ToString(),
            null, cancellationToken);
}