using discipline.centre.shared.abstractions.Exceptions;

namespace discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

public sealed record UserCalendarId(Ulid Value) : ITypeId<UserCalendarId, Ulid>
{
    public static UserCalendarId New() => new(Ulid.NewUlid());

    public static UserCalendarId Parse(string stringTypedId)
    {
        if (!Ulid.TryParse(stringTypedId, out var parsedId))
        {
            throw new InvalidArgumentException("UserCalendarId.InvalidFormat");
        }

        return new UserCalendarId(parsedId);
    }
}