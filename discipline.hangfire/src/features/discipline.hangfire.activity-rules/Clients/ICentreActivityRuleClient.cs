using Refit;

namespace discipline.hangfire.activity_rules.Clients;

[Headers("Content-Type: application/json")]
internal interface ICentreActivityRuleClient
{
    [Get("/activity-rules-module/activity-rules-internal/{userId}/{activityRuleId}")]
    Task<HttpResponseMessage> GetActivityRule(string userId, string activityRuleId, CancellationToken cancellationToken); 
}