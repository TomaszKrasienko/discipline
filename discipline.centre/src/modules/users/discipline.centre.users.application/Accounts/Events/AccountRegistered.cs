using discipline.centre.shared.abstractions.Events;

namespace discipline.centre.users.application.Accounts.Events;

public sealed record AccountRegistered(
    string AccountId,
    string Email) : IEvent;