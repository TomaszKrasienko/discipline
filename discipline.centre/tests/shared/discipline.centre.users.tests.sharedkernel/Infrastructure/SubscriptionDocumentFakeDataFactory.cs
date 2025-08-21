using Bogus;
using discipline.centre.users.domain.Subscriptions.Enums;
using discipline.centre.users.infrastructure.DAL.Subscriptions.Documents;

namespace discipline.centre.users.tests.sharedkernel.Infrastructure;

public static class SubscriptionDocumentFakeDataFactory
{
    internal static SubscriptionDocument Get(decimal perMonth = 0, decimal perYear = 0)
    {
        var types = SubscriptionType
            .GetAvailable()
            .Select(x => x.Value)
            .ToList();
        
        var faker = new Faker<SubscriptionDocument>()
            .RuleFor(f => f.Id, Ulid.NewUlid().ToString())
            .RuleFor(f => f.Type, v => v.PickRandom(types))
            .RuleFor(f => f.Prices, [GetPriceDocument(Currency.Pln)]);
        
        return faker.Generate();
    }

    private static PriceDocument GetPriceDocument(Currency currency)
    {
        var faker = new Faker();
        
        var pricePerMonth = faker.Random.Decimal(min: 10, max: 30);
        var pricePerYear = pricePerMonth * faker.Random.Int(min: 10, max: 13);
        return new PriceDocument()
        {
            PerMonth = pricePerMonth,
            PerYear = pricePerYear,
            Currency = currency.Shorcut
        };
    }
}