using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.centre.users.domain.Accounts.ValueObjects.SubscriptionOrder;

public sealed class SubscriptionDetails : ValueObject
{
    public string Type { get; }
    public int? ValidityPeriod { get; }
    public bool RequirePayment { get; }

    private SubscriptionDetails(
        string type,
        int? validityPeriod,
        bool requirePayment)
    {
        Type = type;
        ValidityPeriod = validityPeriod;
        RequirePayment = requirePayment;
    }

    internal static SubscriptionDetails Create(
        string type,
        int? validityPeriod,
        bool requirePayment)
        => new(type, validityPeriod, requirePayment);

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Type;
        yield return ValidityPeriod;
    }
}