using discipline.hangfire.activity_rules.DAL;
using discipline.hangfire.shared.abstractions.Api;
using discipline.hangfire.shared.abstractions.Identifiers;
using discipline.hangfire.shared.abstractions.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace discipline.hangfire.activity_rules;

internal sealed class ActivityRulesApi(
    ILogger<ActivityRulesApi> logger,
    ActivityRuleDbContext context) : IActivityRulesApi
{
    public async Task<IReadOnlyCollection<ActivityRuleViewModel>> GetActivityRulesByModesAsync(
        IReadOnlyList<string> modes,
        int selectedDay,
        CancellationToken cancellationToken)
    {
        var activityRules = await context.Set<ActivityRuleViewModel>()
            .Where(x 
                => modes.Contains(x.Mode)
                || (x.Mode == "Custom" && x.SelectedDays!.Any(y => y == selectedDay)))
            .ToListAsync(cancellationToken);

        return activityRules.ToArray();
    }

    public async Task<IReadOnlyCollection<AccountId>> GetAccountIdsWithActivityRulesAsync(CancellationToken cancellationToken = default)
        => await context
            .Set<ActivityRuleViewModel>()
            .Select(x => x.AccountId)
            .Distinct()
            .ToListAsync(cancellationToken);
}