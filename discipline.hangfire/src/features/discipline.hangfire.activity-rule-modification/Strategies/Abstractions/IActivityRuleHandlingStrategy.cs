using discipline.hangfire.activity_rule_modification.Events.External;

namespace discipline.hangfire.activity_rule_modification.Strategies.Abstractions;

internal interface IActivityRuleHandlingStrategy
{
    Task HandleAsync(ActivityRuleModified @event, CancellationToken cancellationToken);
    bool CanBeApplied(string messageType);
}