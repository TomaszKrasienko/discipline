using System.Net;
using System.Net.Http.Json;
using discipline.ui.communication.http.ActivityRules;
using discipline.ui.communication.http.ActivityRules.DTOs.Responses;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace discipline.ui.infrastructure.ActivityRules.Facades;

public interface IActivityRulesBrowseFacade
{
    Task<OneOf<IReadOnlyList<ActivityRuleResponseDto>, string>> HandleAsync(CancellationToken cancellationToken);
}

internal sealed class ActivityRulesBrowseFacade(
    IActivityRulesHttpService activityRulesHttpService,
    NavigationManager navigationManager) : IActivityRulesBrowseFacade
{
    public async Task<OneOf<IReadOnlyList<ActivityRuleResponseDto>, string>> HandleAsync(CancellationToken cancellationToken)
    {
        var response = await activityRulesHttpService.GetActivityRules(cancellationToken);
        
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return "user.unauthorized";
        }
        
        var result = await response.Content.ReadFromJsonAsync<IReadOnlyList<ActivityRuleResponseDto>>(cancellationToken)
            ?? [];
        
        return OneOf<IReadOnlyList<ActivityRuleResponseDto>, string>.FromT0(result);
    }
}