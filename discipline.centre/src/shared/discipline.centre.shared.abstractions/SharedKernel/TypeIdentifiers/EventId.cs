using discipline.centre.shared.abstractions.Exceptions;

namespace discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

public sealed record EventId(Ulid Value) : ITypeId<EventId, Ulid>
{
    public static EventId New() => new(Ulid.NewUlid());

    public static EventId Parse(string stringTypedId)
    {
        if (!Ulid.TryParse(stringTypedId, out var parsedId))
        {
            throw new InvalidArgumentException("EventId.InvalidFormat");
        }

        return new EventId(parsedId);
    }
}