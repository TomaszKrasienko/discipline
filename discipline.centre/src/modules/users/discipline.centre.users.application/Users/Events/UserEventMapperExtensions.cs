using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.users.domain.Users.Events;
using discipline.libs.events.abstractions;

namespace discipline.centre.users.application.Users.Events;

internal static class UserEventMapperExtensions
{
    public static IEvent MapAsIntegrationEvent(this DomainEvent domainEvent) => domainEvent switch
    {
        UserCreated @event => new UserSignedUp(@event.UserId, @event.Email.Value),
        _ => throw new InvalidOperationException("Unknown event type")
    };
}