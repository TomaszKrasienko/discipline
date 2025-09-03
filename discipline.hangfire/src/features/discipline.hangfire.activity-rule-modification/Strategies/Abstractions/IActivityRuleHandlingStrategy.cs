using discipline.hangfire.activity_rules.Events.External;

namespace discipline.hangfire.activity_rules.Strategies.Abstractions;

internal interface IActivityRuleHandlingStrategy
{
    Task HandleAsync(ActivityRuleModified @event, CancellationToken cancellationToken);
    bool CanBeApplied(string messageType);
}