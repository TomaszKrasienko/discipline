using discipline.hangfire.activity_rules.DAL;
using discipline.hangfire.activity_rules.Facades;
using discipline.hangfire.activity_rules.Models;
using discipline.hangfire.shared.abstractions.Api;
using discipline.hangfire.shared.abstractions.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace discipline.hangfire.activity_rules;

internal sealed class ActivityRulesApi(ILogger<ActivityRulesApi> logger,
    ICentreFacade centreFacade,
    ActivityRuleDbContext context) : IActivityRulesApi
{
    public async Task GetIncorrectActivityRulesAsync(CancellationToken cancellationToken)
    {        
        var activityRules = await context.Set<ActivityRule>()
            .Where(x => x.Mode == null)
            .ToListAsync(cancellationToken);

        foreach (var activityRule in activityRules)
        {
            var activityRuleResult = await centreFacade.GetActivityRule(activityRule.UserId, 
                activityRule.ActivityRuleId, cancellationToken);

            if (activityRuleResult is { IsT0: true, AsT0: false })
            {
                return;
            }
            
            activityRule.Set(activityRuleResult.AsT1.Details.Title,
                activityRuleResult.AsT1.Mode.Mode,
                activityRuleResult.AsT1.Mode.Days);
        }
        
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<ActivityRuleViewModel>> GetActivityRulesByModesAsync(IReadOnlyList<string> modes, int selectedDay, CancellationToken cancellationToken)
    {
        var activityRules = await context.Set<ActivityRuleViewModel>()
            .Where(x 
                => modes.Contains(x.Mode)
                || (x.Mode == "Custom" && x.SelectedDays!.Any(y => y == selectedDay)))
            .ToListAsync(cancellationToken);

        return activityRules.ToArray();
    }
}