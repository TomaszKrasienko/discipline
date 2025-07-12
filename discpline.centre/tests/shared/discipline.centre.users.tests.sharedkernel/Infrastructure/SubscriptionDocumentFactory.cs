using Bogus;
using discipline.centre.users.infrastructure.DAL.Subscriptions.Documents;

namespace discipline.centre.users.tests.sharedkernel.Infrastructure;

public static class SubscriptionDocumentFactory
{
    internal static SubscriptionDocument Get(decimal perMonth = 0, decimal perYear = 0)
    {
        var faker = new Faker<SubscriptionDocument>()
            .RuleFor(f => f.Id, Ulid.NewUlid().ToString())
            .RuleFor(f => f.Title, v => v.Lorem.Word())
            .RuleFor(f => f.PricePerMonth, perMonth)
            .RuleFor(f => f.PricePerYear, perYear)
            .RuleFor(f => f.Features, v => [v.Random.String(minChar: 'a', maxChar: 'z')]);
        
        return faker.Generate();
    }
}