using discipline.hangfire.shared.abstractions.Identifiers;
using discipline.hangfire.shared.abstractions.Messaging;

namespace discipline.hangfire.create_activity_from_planned.Commands;

internal sealed record CreateActivityFromActivityRuleCommand(ActivityRuleId ActivityRuleId, UserId UserId) : IMessage;