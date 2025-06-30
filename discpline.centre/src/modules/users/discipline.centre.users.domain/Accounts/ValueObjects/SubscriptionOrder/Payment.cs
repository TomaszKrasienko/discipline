using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.centre.users.domain.Accounts.ValueObjects.SubscriptionOrder;

public sealed class Payment : ValueObject
{
    public DateTimeOffset CreatedAt { get; }
    public decimal Value { get; }

    private Payment(
        DateTimeOffset createdAt,
        decimal value)
    {
        CreatedAt = createdAt;
        Value = value;
    }

    internal static Payment Create(
        TimeProvider timeProvider,
        decimal value)
        => new Payment(timeProvider.GetUtcNow(), value);
    

    protected override IEnumerable<object?> GetAtomicValues()
    {
        throw new NotImplementedException();
    }
}