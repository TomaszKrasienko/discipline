using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.users.domain.Users.Events;

public sealed record SubscriptionCreated(SubscriptionId Id, string Title) : DomainEvent;