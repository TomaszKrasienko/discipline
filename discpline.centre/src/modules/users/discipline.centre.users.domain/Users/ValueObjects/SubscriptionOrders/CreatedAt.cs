using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.users.domain.Users.Rules.SubscriptionOrders;

namespace discipline.centre.users.domain.Users.ValueObjects.SubscriptionOrders;

public sealed class CreatedAt : ValueObject
{
    private readonly DateTimeOffset _value;
    public DateTimeOffset Value
    {
        get => _value;
        private init
        {
            CheckRule(new CreatedAtCanNoBeDefaultRule(value));
            _value = value;
        }
    }

    public static CreatedAt Create(DateTimeOffset value) => new(value);
    
    private CreatedAt(DateTimeOffset value) => Value = value;

    public static implicit operator DateTimeOffset(CreatedAt createdAt) => createdAt.Value;

    public static implicit operator CreatedAt(DateTimeOffset value) => Create(value);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}