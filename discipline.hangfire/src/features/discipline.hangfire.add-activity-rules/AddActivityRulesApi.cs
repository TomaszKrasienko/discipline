using discipline.hangfire.add_activity_rules.DAL;
using discipline.hangfire.add_activity_rules.Facades;
using discipline.hangfire.add_activity_rules.Models;
using discipline.hangfire.shared.abstractions.Api;
using discipline.hangfire.shared.abstractions.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace discipline.hangfire.add_activity_rules;

internal sealed class AddActivityRulesApi(ILogger<AddActivityRulesApi> logger,
    ICentreFacade centreFacade,
    AddActivityRuleDbContext context) : IAddActivityRulesApi
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
            
            activityRule.Set(activityRuleResult.AsT1.Mode,  activityRuleResult.AsT1.SelectedDays);
        }
        
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<ActivityRuleViewModel>> GetActivityRulesByModesAsync(IReadOnlyList<string> modes, int selectedDay, CancellationToken cancellationToken)
    {
        var activityRules = await context.Set<ActivityRuleViewModel>()
            .Where(x 
                => modes.Contains(x.Mode)
                || (x.Mode == "Custom" && x.SelectedDays.Any(y => y == selectedDay)))
            .ToListAsync(cancellationToken);

        return activityRules.ToArray();
    }
}