using System.Net;
using System.Net.Http.Json;
using discipline.ui.communication.http.ActivityRules;
using OneOf;
using Refit;

namespace discipline.ui.infrastructure.ActivityRules.Facades;

public interface IDeleteActivityRuleFacade
{
    Task<OneOf<bool, string>> HandleAsync(string activityRuleId, CancellationToken cancellationToken);
}

internal sealed class DeleteActivityRuleFacade(
    IActivityRulesHttpService activityRulesHttpService) : IDeleteActivityRuleFacade
{
    public async Task<OneOf<bool, string>> HandleAsync(string activityRuleId, CancellationToken cancellationToken)
    {
        var response = await activityRulesHttpService.DeleteActivityRule(activityRuleId, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return "user.unauthorized";
            }
            
            var error = await response.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken);

            return error?.Title ?? "system.unexpectedError";
        }

        return true;
    }
}