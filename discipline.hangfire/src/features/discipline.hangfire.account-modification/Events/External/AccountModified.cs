using discipline.hangfire.shared.abstractions.Events;

namespace discipline.hangfire.account_modification.Events.External;

public sealed record AccountModified(string AccountId) : IEvent;