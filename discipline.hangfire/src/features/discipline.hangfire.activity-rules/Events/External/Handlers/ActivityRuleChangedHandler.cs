using discipline.hangfire.activity_rules.DAL;
using discipline.hangfire.activity_rules.Models;
using discipline.hangfire.shared.abstractions.Events;
using discipline.hangfire.shared.abstractions.Identifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace discipline.hangfire.activity_rules.Events.External.Handlers;

internal sealed class ActivityRuleChangedHandler(
    ILogger<ActivityRuleChangedHandler> logger,
    ActivityRuleDbContext context) : IEventHandler<ActivityRuleChanged>
{
    public async Task HandleAsync(ActivityRuleChanged @event, CancellationToken cancellationToken)
    {
        var stronglyActivityRuleId = ActivityRuleId.Parse(@event.ActivityRuleId);
        var stronglyUserId = UserId.Parse(@event.UserId);
        
        var activityRule = await context.Set<ActivityRule>()
            .SingleOrDefaultAsync(x 
                => x.ActivityRuleId == stronglyActivityRuleId
                && x.UserId == stronglyUserId, cancellationToken);

        activityRule?.UpdateMode(
            @event.Title,
            @event.Mode,
            @event.Days);
        
        await context.SaveChangesAsync(cancellationToken);
    }
}