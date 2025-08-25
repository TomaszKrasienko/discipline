using discipline.hangfire.activity_rules.DAL.Repositories;
using discipline.hangfire.activity_rules.Models;
using discipline.hangfire.shared.abstractions.Events;
using discipline.hangfire.shared.abstractions.Identifiers;
using Microsoft.Extensions.Logging;

namespace discipline.hangfire.activity_rules.Events.External.Handlers;

internal sealed class ActivityRuleRegisteredHandler(
    ILogger<ActivityRuleRegisteredHandler> logger,
    IActivityRuleRepository repository) : IEventHandler<ActivityRuleRegistered>
{
    public async Task HandleAsync(ActivityRuleRegistered @event, CancellationToken cancellationToken)
    {
        logger.LogInformation("ActivityRuleRegisteredHandler.HandleAsync called");
        
        var stronglyActivityRuleId = ActivityRuleId.Parse(@event.ActivityRuleId);
        var stronglyUserId = UserId.Parse(@event.UserId);

        if (await repository.DoesActivityRuleExistAsync(
                stronglyActivityRuleId,
                stronglyUserId,
                cancellationToken))
        {
            return;
        }

        var activityRule = ActivityRule.Create(
            stronglyActivityRuleId,
            stronglyUserId,
            @event.Title,
            @event.Mode,
            @event.Days);
        
        await repository.AddAsync(activityRule, cancellationToken);
    }
}