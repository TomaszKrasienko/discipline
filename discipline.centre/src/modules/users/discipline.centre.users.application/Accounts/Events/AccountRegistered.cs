using discipline.libs.events.abstractions;

namespace discipline.centre.users.application.Accounts.Events;

public sealed record AccountRegistered(
    string AccountId,
    string Email) : IEvent;