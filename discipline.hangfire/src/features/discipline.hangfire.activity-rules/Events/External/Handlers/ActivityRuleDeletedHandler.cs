using discipline.hangfire.activity_rules.DAL;
using discipline.hangfire.activity_rules.Models;
using discipline.hangfire.shared.abstractions.Events;
using discipline.hangfire.shared.abstractions.Identifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace discipline.hangfire.activity_rules.Events.External.Handlers;

internal sealed class ActivityRuleDeletedHandler(
    ILogger<ActivityRuleDeletedHandler> logger,
    ActivityRuleDbContext context) : IEventHandler<ActivityRuleDeleted>
{
    public async Task HandleAsync(ActivityRuleDeleted @event, CancellationToken cancellationToken)
    {
        var stronglyActivityRuleId = ActivityRuleId.Parse(@event.ActivityRuleId);
        var stronglyUserId = UserId.Parse(@event.UserId);
        
        var activityRule = await context.Set<ActivityRule>()
            .SingleOrDefaultAsync(x 
                => x.ActivityRuleId == stronglyActivityRuleId
                   && x.UserId == stronglyUserId, cancellationToken);

        if (activityRule is null)
        {
            return;
        }
        
        context.Remove(activityRule);
        await context.SaveChangesAsync(cancellationToken);
    }
}