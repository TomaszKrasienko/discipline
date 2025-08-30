using discipline.hangfire.activity_rules.DAL;
using discipline.hangfire.activity_rules.Events.External;
using discipline.hangfire.activity_rules.Models;
using discipline.hangfire.activity_rules.Strategies.Abstractions;
using discipline.hangfire.shared.abstractions.Identifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace discipline.hangfire.activity_rules.Strategies;

internal sealed class ActivityRuleCreatedStrategy(
    ILogger<ActivityRuleCreatedStrategy> logger,
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
        var stronglyUserId = UserId.Parse(@event.UserId);
        
        var doesActivityRuleExist = await context.Set<ActivityRule>()
            .AnyAsync(x 
                => x.ActivityRuleId == stronglyActivityRuleId && 
                   x.UserId == stronglyUserId, cancellationToken);

        if (doesActivityRuleExist)
        {
            logger.LogInformation($"Activity rule {stronglyActivityRuleId} already exists");
        }

        var activityRule = ActivityRule.Create(
            stronglyActivityRuleId,
            stronglyUserId,
            @event.Title,
            @event.Mode,
            @event.Days);
        
        context.Set<ActivityRule>().Add(activityRule);
        await context.SaveChangesAsync(cancellationToken);
    }

    public bool CanBeApplied(string messageType)
        => messageType.EndsWith("Registered");
}