using discipline.activity_scheduler.activity_rule_modification.Events.External;
using discipline.activity_scheduler.activity_rule_modification.Strategies.Abstractions;
using discipline.activity_scheduler.domain.ActivityRules;
using discipline.activity_scheduler.shared.abstractions.DAL;
using discipline.activity_scheduler.shared.abstractions.Identifiers;
using Microsoft.Extensions.Logging;

namespace discipline.activity_scheduler.activity_rule_modification.Strategies;

internal sealed class ActivityRuleRegisteredStrategy(
    ILogger<ActivityRuleRegisteredStrategy> logger,
    IWriteRepository<ActivityRule, ActivityRuleId> activityRuleRepository) : IActivityRuleHandlingStrategy
{
    public async Task HandleAsync(ActivityRuleModified @event, CancellationToken cancellationToken)
    {
        if (@event.Title is null ||
            @event.Mode is null)
        {
            throw new ArgumentException("Title and Mode cannot be null");
        }
        
        var stronglyActivityRuleId = ActivityRuleId.Parse(@event.ActivityRuleId);
        var stronglyUserId = AccountId.Parse(@event.UserId);
        
        var doesActivityRuleExist = await activityRuleRepository.DoesExistAsync(x 
                => x.ActivityRuleId == stronglyActivityRuleId &&  
                   x.AccountId == stronglyUserId, cancellationToken);

        if (doesActivityRuleExist)
        {
            logger.LogWarning($"Activity rule {stronglyActivityRuleId} already exists");            
            return;
        }

        var activityRule = ActivityRule.Create(
            stronglyActivityRuleId,
            stronglyUserId,
            @event.Title,
            @event.Mode,
            @event.Days);
        
        await activityRuleRepository.AddAsync(activityRule, cancellationToken);
    }

    public bool CanBeApplied(string messageType)
        => messageType.EndsWith("Registered");
}