using discipline.libs.messaging.Abstractions;

namespace discipline.centre.activityrules.application.ActivityRules.Commands.External;

public sealed record CreateActivityForAccountFromActivityRuleCommand(
    IReadOnlyCollection<string> ActivityRuleId,
    string AccountId) : IMessage;