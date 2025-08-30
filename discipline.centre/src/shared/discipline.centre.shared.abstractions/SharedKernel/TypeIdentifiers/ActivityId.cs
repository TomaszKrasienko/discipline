using discipline.centre.shared.abstractions.Exceptions;

namespace discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

public sealed record ActivityId(Ulid Value) : ITypeId<ActivityId, Ulid>
{
    public static ActivityId New() => new(Ulid.NewUlid());

    public static ActivityId Parse(string stringTypedId)
    {
        if (!Ulid.TryParse(stringTypedId, out var parsedId))
        {
            throw new InvalidArgumentException("ActivityId.InvalidFormat");
        }

        return new ActivityId(parsedId);
    }

    public override string ToString() => Value.ToString();
}