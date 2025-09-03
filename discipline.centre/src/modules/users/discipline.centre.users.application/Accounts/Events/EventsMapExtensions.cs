using discipline.centre.shared.abstractions.Events;
using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.users.domain.Accounts.Events;

namespace discipline.centre.users.application.Accounts.Events;

internal static class EventsMapExtensions
{
    //TODO: Unit tests
    internal static IEvent MapAsIntegrationEvent(this DomainEvent domainEvent) => domainEvent switch
    {
        AccountCreated @event => new AccountRegistered(
            @event.AccountId.ToString(),
            @event.Login.Value),
        _ => throw new InvalidOperationException("Unknown event type")
    };
}