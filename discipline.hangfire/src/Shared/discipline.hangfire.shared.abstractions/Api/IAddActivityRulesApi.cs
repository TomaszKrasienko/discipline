using discipline.hangfire.shared.abstractions.ViewModels;

namespace discipline.hangfire.shared.abstractions.Api;

public interface IAddActivityRulesApi
{
    Task GetIncorrectActivityRulesAsync(CancellationToken cancellationToken);
    Task<IReadOnlyCollection<ActivityRuleViewModel>> GetActivityRulesByModesAsync(IReadOnlyList<string> modes, int selectedDay, CancellationToken cancellationToken);
}