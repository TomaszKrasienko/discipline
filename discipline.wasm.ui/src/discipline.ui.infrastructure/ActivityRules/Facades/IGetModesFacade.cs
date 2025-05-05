using System.Net;
using System.Net.Http.Json;
using discipline.ui.communication.http.ActivityRules;
using discipline.ui.communication.http.ActivityRules.DTOs.Responses;
using OneOf;

namespace discipline.ui.infrastructure.ActivityRules.Facades;

public interface IGetModesFacade
{
    Task<OneOf<IReadOnlyList<ModeResponseDto>, string>> HandelAsync(CancellationToken cancellationToken);
}

internal sealed class GetModesFacade(
    IActivityRulesHttpService activityRulesHttpService) : IGetModesFacade
{
    public async Task<OneOf<IReadOnlyList<ModeResponseDto>, string>> HandelAsync(CancellationToken cancellationToken)
    {
        var response = await activityRulesHttpService.GetModesAsync(cancellationToken);
        
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return "user.unauthorized";
        }
        
        var result = await response.Content.ReadFromJsonAsync<IReadOnlyList<ModeResponseDto>>(cancellationToken)
                     ?? [];
        
        return OneOf<IReadOnlyList<ModeResponseDto>, string>.FromT0(result);
    }
}