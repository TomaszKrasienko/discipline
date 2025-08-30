using discipline.centre.shared.abstractions.Exceptions;

namespace discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

public readonly record struct SubscriptionId(Ulid Value) : ITypeId<SubscriptionId, Ulid>
{
    public static SubscriptionId New() => new(Ulid.NewUlid());
    
    public static SubscriptionId Parse(string stringTypedId)
    {
        if (!Ulid.TryParse(stringTypedId, out var parsedId))
        {
            throw new InvalidArgumentException("SubscriptionId.InvalidFormat");
        }

        return new SubscriptionId(parsedId);
    }

    public override string ToString()
        => Value.ToString();
}