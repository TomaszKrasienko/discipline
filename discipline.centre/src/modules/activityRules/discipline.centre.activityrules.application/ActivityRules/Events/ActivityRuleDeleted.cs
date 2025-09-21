using discipline.libs.events.abstractions;

namespace discipline.centre.activityrules.application.ActivityRules.Events;

public sealed record ActivityRuleDeleted(Ulid UserId, Ulid ActivityRuleId) : IEvent;