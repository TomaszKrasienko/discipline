using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.users.domain.Accounts.Rules.Payments;

namespace discipline.centre.users.domain.Accounts.ValueObjects.SubscriptionOrder;

public sealed class Payment : ValueObject
{
    private readonly decimal _value;
    
    public DateTimeOffset CreatedAt { get; }

    public decimal Value
    {
        private init
        {
            CheckRule(new PaymentValueCannotBeNegativeOrZero(value));
            _value = value;
        }
        get => _value;
    }

    private Payment(
        DateTimeOffset createdAt,
        decimal value)
    {
        CreatedAt = createdAt;
        Value = value;
    }

    internal static Payment Create(TimeProvider timeProvider, decimal value) 
        => new(timeProvider.GetUtcNow(), value);
    

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return CreatedAt;
        yield return Value;
    }
}