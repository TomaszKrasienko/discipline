using discipline.hangfire.activity_rules.DAL;
using discipline.hangfire.activity_rules.DAL.Repositories;
using discipline.hangfire.activity_rules.Models;
using discipline.hangfire.shared.abstractions.Events;
using discipline.hangfire.shared.abstractions.Identifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace discipline.hangfire.activity_rules.Events.External.Handlers;

internal sealed class ActivityRuleDeletedHandler(
    ILogger<ActivityRuleDeletedHandler> logger,
    IActivityRuleRepository activityRuleRepository) : IEventHandler<ActivityRuleDeleted>
{
    public async Task HandleAsync(ActivityRuleDeleted @event, CancellationToken cancellationToken)
    {
        logger.LogInformation("ActivityRuleDeletedHandler.HandleAsync called");
        
        var stronglyActivityRuleId = ActivityRuleId.Parse(@event.ActivityRuleId);
        var stronglyUserId = UserId.Parse(@event.UserId);
        
        var activityRule = await activityRuleRepository
            .GetByIdAsync(
                stronglyActivityRuleId,
                stronglyUserId,
                cancellationToken);

        if (activityRule is null)
        {
            return;
        }
        
        await activityRuleRepository.DeleteAsync(
            activityRule,
            cancellationToken);
    }
}