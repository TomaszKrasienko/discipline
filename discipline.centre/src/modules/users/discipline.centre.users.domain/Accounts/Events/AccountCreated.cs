using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Accounts.ValueObjects.Account;

namespace discipline.centre.users.domain.Accounts.Events;

public sealed record AccountCreated(
    AccountId AccountId,
    Login Login) : DomainEvent;