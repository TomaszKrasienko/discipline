using discipline.hangfire.activity_rules.Models;
using discipline.hangfire.shared.abstractions.Identifiers;
using Microsoft.EntityFrameworkCore;

namespace discipline.hangfire.activity_rules.DAL.Repositories;

internal interface IActivityRuleRepository
{
    Task AddAsync(
        ActivityRule activityRule,
        CancellationToken cancellationToken = default);
    
    Task DeleteAsync(
        ActivityRule activityRule,
        CancellationToken cancellationToken = default);
    
    Task<ActivityRule?> GetByIdAsync(
        ActivityRuleId activityRuleId,
        UserId userId,
        CancellationToken cancellationToken = default);
    
    Task<bool> DoesActivityRuleExistAsync(
        ActivityRuleId  activityRuleId,
        UserId userId,
        CancellationToken cancellationToken = default);
}

internal sealed class ActivityRuleRepository(
    ActivityRuleDbContext context) : IActivityRuleRepository
{
    private readonly DbSet<ActivityRule> _activityRuleSet = context.Set<ActivityRule>();
    
    public async Task AddAsync(
        ActivityRule activityRule,
        CancellationToken cancellationToken = default)
    {
        _activityRuleSet.Add(activityRule);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        ActivityRule activityRule,
        CancellationToken cancellationToken = default)
    {
        _activityRuleSet.Remove(activityRule);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<ActivityRule?> GetByIdAsync(
        ActivityRuleId activityRuleId,
        UserId userId,
        CancellationToken cancellationToken = default)
        => await _activityRuleSet
            .FindAsync(
                keyValues: [activityRuleId, userId],
                cancellationToken: cancellationToken);

    public Task<bool> DoesActivityRuleExistAsync(
        ActivityRuleId activityRuleId,
        UserId userId,
        CancellationToken cancellationToken = default)
        => _activityRuleSet
            .AnyAsync(r 
                => r.ActivityRuleId == activityRuleId &&
                   r.UserId == userId, cancellationToken);
}