using discipline.activity_scheduler.shared.abstractions.Identifiers;
using discipline.libs.messaging.Abstractions;

namespace discipline.activity_scheduler.create_activity_from_planned.Commands;

internal sealed record CreateActivityForAccountFromActivityRuleCommand(
    IReadOnlyCollection<ActivityRuleId> ActivityRuleId,
    AccountId AccountId) : IMessage;