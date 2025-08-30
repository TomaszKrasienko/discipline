using discipline.hangfire.activity_rules.DAL;
using discipline.hangfire.activity_rules.Events.External;
using discipline.hangfire.activity_rules.Models;
using discipline.hangfire.activity_rules.Strategies.Abstractions;
using discipline.hangfire.shared.abstractions.Identifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace discipline.hangfire.activity_rules.Strategies;

internal sealed class ActivityRuleDeletedStrategy(
    ILogger<ActivityRuleDeletedStrategy> logger,
    ActivityRuleDbContext context) : IActivityRuleHandlingStrategy
{
    public async Task HandleAsync(ActivityRuleModified @event, CancellationToken cancellationToken)
    {
        var stronglyActivityRuleId = ActivityRuleId.Parse(@event.ActivityRuleId);
        var stronglyUserId = UserId.Parse(@event.UserId);
        
        var activityRule = await context.Set<ActivityRule>()
            .SingleOrDefaultAsync(x 
                => x.ActivityRuleId == stronglyActivityRuleId && 
                   x.UserId == stronglyUserId, cancellationToken);

        if (activityRule is null)
        {
            logger.LogInformation($"Activity rule {stronglyActivityRuleId} not found");
            return;
        }
        
        context.Set<ActivityRule>().Remove(activityRule);
        await context.SaveChangesAsync(cancellationToken);
    }

    public bool CanBeApplied(string messageType)
        => messageType.EndsWith("Deleted");
}