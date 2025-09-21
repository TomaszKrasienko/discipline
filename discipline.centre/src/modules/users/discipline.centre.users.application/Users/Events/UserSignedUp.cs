using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.libs.events.abstractions;

namespace discipline.centre.users.application.Users.Events;

public sealed record UserSignedUp(UserId UserId, string Email) : IEvent;