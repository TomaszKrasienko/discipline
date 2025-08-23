using discipline.centre.shared.abstractions.Exceptions;

namespace discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

public readonly record struct UserId(Ulid Value) : ITypeId<UserId, Ulid>
{
    public static UserId New() => new(Ulid.NewUlid());

    public static UserId Parse(string stringTypedId)
    {
        if (!Ulid.TryParse(stringTypedId, out var parsedId))
        {
            throw new InvalidArgumentException("UserId.InvalidFormat");
        }

        return new UserId(parsedId);
    }

    public override string ToString() => Value.ToString();
}