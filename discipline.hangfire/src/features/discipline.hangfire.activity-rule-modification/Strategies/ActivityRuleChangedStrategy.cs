using discipline.hangfire.activity_rules.DAL;
using discipline.hangfire.activity_rules.Events.External;
using discipline.hangfire.activity_rules.Models;
using discipline.hangfire.activity_rules.Strategies.Abstractions;
using discipline.hangfire.shared.abstractions.Identifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace discipline.hangfire.activity_rules.Strategies;

internal sealed class ActivityRuleChangedStrategy(
    ILogger<ActivityRuleChangedStrategy> logger,
    ActivityRuleDbContext context) : IActivityRuleHandlingStrategy
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
        
        var activityRule = await context.Set<ActivityRule>()
            .SingleOrDefaultAsync(x 
                => x.ActivityRuleId == stronglyActivityRuleId && 
                   x.AccountId == stronglyUserId, cancellationToken);

        if (activityRule is null)
        {
            logger.LogInformation($"Activity rule {stronglyActivityRuleId} not found");
            return;
        }

        activityRule.UpdateMode(
            @event.Title,
            @event.Mode,
            @event.Days);
        
        await context.SaveChangesAsync(cancellationToken);
    }

    public bool CanBeApplied(string messageType)
        => messageType.EndsWith("Changed");
}