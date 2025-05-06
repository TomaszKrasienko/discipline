using System.Net.Http.Json;
using discipline.hangfire.add_planned_tasks.Clients;
using discipline.hangfire.add_planned_tasks.DTOs;
using discipline.hangfire.shared.abstractions.Time;
using Microsoft.Extensions.Logging;
using OneOf;

namespace discipline.hangfire.add_planned_tasks.Facades;

internal interface ICentreFacade
{
    Task<OneOf<bool, ActiveModesDto>> GetActiveModes(CancellationToken cancellationToken);
}

internal sealed class CentreFacade(ILogger<CentreFacade> logger,
    IClock clock,
    ICentreActivityRuleClient centreActivityRuleClient) : ICentreFacade
{
    public async Task<OneOf<bool, ActiveModesDto>> GetActiveModes(CancellationToken cancellationToken)
    {
        var now = clock.Now();

        var activityModesResponse = await centreActivityRuleClient.GetActiveModeAsync(now.AddDays(1).ToString("yyyy-MM-dd"));

        if (!activityModesResponse.IsSuccessStatusCode)
        {
            logger.LogError("Failed to get active modes");
            return false;
        }
        
        var activityModes = await  activityModesResponse.Content.ReadFromJsonAsync<ActiveModesDto>(cancellationToken);

        if (activityModes is null)
        {
            logger.LogError("Failed to get active modes");
            return false;
        }

        return activityModes;
    }
}