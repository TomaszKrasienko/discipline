using Refit;

namespace discipline.hangfire.add_planned_tasks.Clients;

[Headers("Content-Type: application/json")]
internal interface ICentreActivityRuleClient
{
    [Get("/activity-rules-module/activity-rules-internal/modes")]
    Task<HttpResponseMessage> GetActiveModeAsync(string day); 
}