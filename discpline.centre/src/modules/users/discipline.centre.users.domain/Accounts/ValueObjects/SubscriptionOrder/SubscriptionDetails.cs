using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.users.domain.Subscriptions.Enums;

namespace discipline.centre.users.domain.Accounts.ValueObjects.SubscriptionOrder;

public sealed class SubscriptionDetails : ValueObject
{
    public string Type { get; }
    public Period? ValidityPeriod { get; }
    public bool RequirePayment { get; }

    private SubscriptionDetails(
        string type,
        Period? validityPeriod,
        bool requirePayment)
    {
        Type = type;
        ValidityPeriod = validityPeriod;
        RequirePayment = requirePayment;
    }

    public static SubscriptionDetails Create(
        string type,
        Period? validityPeriod,
        bool requirePayment)
        => new(type, validityPeriod, requirePayment);

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Type;
        yield return ValidityPeriod;
        yield return RequirePayment;
    }
}