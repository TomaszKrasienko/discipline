using discipline.hangfire.activity_rules.DAL;
using discipline.hangfire.activity_rules.Models;
using discipline.hangfire.shared.abstractions.Events;
using discipline.hangfire.shared.abstractions.Identifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace discipline.hangfire.activity_rules.Events.External.Handlers;

internal sealed class ActivityRuleModeChangedHandler(
    ILogger<ActivityRuleModeChangedHandler> logger,
    ActivityRuleDbContext context) : IEventHandler<ActivityRuleModeChanged>
{
    public async Task HandleAsync(ActivityRuleModeChanged @event, CancellationToken cancellationToken)
    {
        var stronglyActivityRuleId = ActivityRuleId.Parse(@event.ActivityRuleId);
        var stronglyUserId = UserId.Parse(@event.UserId);
        
        var activityRule = await context.Set<ActivityRule>()
            .SingleOrDefaultAsync(x 
                => x.ActivityRuleId == stronglyActivityRuleId
                && x.UserId == stronglyUserId, cancellationToken);

        activityRule?.UpdateMode(@event.Mode, @event.Days);
    }
}