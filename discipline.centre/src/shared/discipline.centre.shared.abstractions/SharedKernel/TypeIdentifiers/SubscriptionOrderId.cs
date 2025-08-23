using discipline.centre.shared.abstractions.Exceptions;

namespace discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

public readonly record struct SubscriptionOrderId(Ulid Value) : ITypeId<SubscriptionOrderId, Ulid>
{
    public static SubscriptionOrderId New() => new(Ulid.NewUlid());

    public static SubscriptionOrderId Parse(string stringTypedId)
    {
        if (!Ulid.TryParse(stringTypedId, out var parsedId))
        {
            throw new InvalidArgumentException("SubscriptionOrderId.InvalidFormat");
        }

        return new SubscriptionOrderId(parsedId);
    }
}