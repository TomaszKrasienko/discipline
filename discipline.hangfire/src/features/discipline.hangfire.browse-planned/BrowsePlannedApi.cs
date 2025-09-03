using System.Collections.Immutable;
using discipline.hangfire.browse_planned.DAL;
using discipline.hangfire.shared.abstractions.Api;
using discipline.hangfire.shared.abstractions.Identifiers;
using discipline.hangfire.shared.abstractions.ViewModels;
using Microsoft.EntityFrameworkCore;
using PlannedTaskViewModel = discipline.hangfire.browse_planned.ViewModels.PlannedTaskViewModel;

namespace discipline.hangfire.browse_planned;

internal sealed class BrowsePlannedApi(
    BrowsePlannedDbContext context) : IBrowsePlannedApi
{
    public async Task<ImmutableDictionary<DateOnly, List<PlannedTaskDetailsViewModel>>> GetPlannedTaskDetailsAsync(
        AccountId accountId, 
        CancellationToken cancellationToken = default)
    {
        var result = await context.Set<PlannedTaskViewModel>()
            .Where(x => x.AccountId == accountId)
            .ToListAsync(cancellationToken);

        return result
            .GroupBy(x => x.PlannedFor)
            .ToImmutableDictionary(
                x => x.Key,
                y => y.Select(Map).ToList());
    }

    private PlannedTaskDetailsViewModel Map(PlannedTaskViewModel vm)
        => new(vm.ActivityRuleId.ToString(), vm.PlannedFor, vm.CreatedAt, vm.IsPlannedEnabled, vm.IsActivityCreated);
}