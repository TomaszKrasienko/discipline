using discipline.hangfire.shared.abstractions.Identifiers;
using discipline.hangfire.shared.abstractions.Messaging;
using discipline.libs.messaging.Abstractions;

namespace discipline.hangfire.create_activity_from_planned.Commands;

internal sealed record CreateActivityFromActivityRuleCommand(
    ActivityRuleId ActivityRuleId,
    AccountId AccountId) : IMessage;