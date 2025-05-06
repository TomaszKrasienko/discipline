using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Json;
using discipline.hangfire.add_activity_rules.Clients;
using discipline.hangfire.add_activity_rules.DAL;
using discipline.hangfire.add_activity_rules.DTOs;
using discipline.hangfire.add_activity_rules.Facades;
using discipline.hangfire.add_activity_rules.Models;
using discipline.hangfire.shared.abstractions.Events;
using discipline.hangfire.shared.abstractions.Identifiers;
using discipline.hangfire.shared.abstractions.Time;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace discipline.hangfire.add_activity_rules.Events.External;

internal sealed class ActivityRuleRegisteredHandler(
    ILogger<ActivityRuleRegisteredHandler> logger,
    ICentreFacade centreFacade,
    AddActivityRuleDbContext context) : IEventHandler<ActivityRuleRegistered>
{
    public async Task HandleAsync(ActivityRuleRegistered @event, CancellationToken cancellationToken)
    {
        var stronglyActivityRuleId = ActivityRuleId.Parse(@event.ActivityRuleId);
        var stronglyUserId = UserId.Parse(@event.UserId);

        var activityRule = ActivityRule.Create(stronglyActivityRuleId, stronglyUserId);
        context.Add(activityRule);
        await context.SaveChangesAsync(cancellationToken);
        
        var activityRuleResult = await centreFacade.GetActivityRule(stronglyUserId,
            stronglyActivityRuleId, cancellationToken);

        if (activityRuleResult is { IsT0: true, AsT0: false })
        {
            return;
        }
        
        var existingRule = await context.Set<ActivityRule>().FindAsync(keyValues: [stronglyActivityRuleId], cancellationToken: cancellationToken);
        existingRule?.Set(activityRuleResult.AsT1.Mode,  activityRuleResult.AsT1.SelectedDays);
        
        await context.SaveChangesAsync(cancellationToken);
    }
}