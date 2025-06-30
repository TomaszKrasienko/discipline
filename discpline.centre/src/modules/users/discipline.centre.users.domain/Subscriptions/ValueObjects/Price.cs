using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.users.domain.Subscriptions.Rules;

namespace discipline.centre.users.domain.Subscriptions.ValueObjects;

public sealed class Price : ValueObject
{
    private readonly decimal _perMonth;
    private readonly decimal _perYear;

    public decimal PerMonth
    {
        get => _perMonth;
        private init
        {
            CheckRule(new PriceCanBeLessThanZeroRule(value, nameof(PerMonth)));
            _perMonth = value;
        }
    }

    public decimal PerYear
    {
        get => _perYear;
        private init
        {
            CheckRule(new PriceCanBeLessThanZeroRule(value, nameof(PerYear)));
            _perYear = value;
        }
    }

    public string Currency { get; private set; }

    public static Price Create(
        decimal perMonth,
        decimal perYear,
        string currency) => new(perMonth, perYear, currency);

    private Price(
        decimal perMonth,
        decimal perYear,
        string currency)
    {
        PerMonth = perMonth;
        PerYear = perYear;
        Currency = currency;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return PerMonth;
        yield return PerYear;
        yield return Currency;
    }
}