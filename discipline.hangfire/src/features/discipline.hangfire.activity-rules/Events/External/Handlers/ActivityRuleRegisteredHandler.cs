using discipline.hangfire.activity_rules.DAL;
using discipline.hangfire.activity_rules.Facades;
using discipline.hangfire.activity_rules.Models;
using discipline.hangfire.shared.abstractions.Events;
using discipline.hangfire.shared.abstractions.Identifiers;
using Microsoft.Extensions.Logging;

namespace discipline.hangfire.activity_rules.Events.External.Handlers;

internal sealed class ActivityRuleRegisteredHandler(
    ILogger<ActivityRuleRegisteredHandler> logger,
    ICentreFacade centreFacade,
    ActivityRuleDbContext context) : IEventHandler<ActivityRuleRegistered>
{
    public async Task HandleAsync(ActivityRuleRegistered @event, CancellationToken cancellationToken)
    {
        var stronglyActivityRuleId = ActivityRuleId.Parse(@event.ActivityRuleId);
        var stronglyUserId = UserId.Parse(@event.UserId);

        var activityRule = ActivityRule.Create(
            stronglyActivityRuleId,
            stronglyUserId,
            @event.Title,
            @event.Mode,
            @event.Days);
        
        context.Add(activityRule);
        await context.SaveChangesAsync(cancellationToken);
    }
}