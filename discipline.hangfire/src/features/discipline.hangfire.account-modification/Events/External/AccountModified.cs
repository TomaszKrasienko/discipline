using discipline.libs.events.abstractions;

namespace discipline.hangfire.account_modification.Events.External;

public sealed record AccountModified(string AccountId) : IEvent;