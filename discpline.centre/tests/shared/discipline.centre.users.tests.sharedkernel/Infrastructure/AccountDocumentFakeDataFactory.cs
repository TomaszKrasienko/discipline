using Bogus;
using discipline.centre.users.infrastructure.DAL.Accounts.Documents;

namespace discipline.centre.users.tests.sharedkernel.Infrastructure;

public static class AccountDocumentFakeDataFactory
{
    public static AccountDocument Get()
    {
        var faker = new Faker();

        return new AccountDocument()
        {
            Id = Ulid.NewUlid().ToString(),
            Login = faker.Internet.Email(),
            HashedPassword = faker.Random.String(),
            SubscriptionOrders = []
        };
    }

    public static AccountDocument WithSubscriptionOrder(
        this AccountDocument accountDocument,
        bool withPayment = false)
    {
        var faker = new Faker();
        var startDate = DateOnly
            .FromDateTime(DateTime.Now.AddDays(faker.Random.Int(min: -30, max: 30)));

        var validityPeriod = faker.Random.Int(min: 1, max: 60);

        var subscription = new SubscriptionOrderDocument()
        {
            Id = Ulid.NewUlid().ToString(),
            StartDate = startDate,
            FinishDate = withPayment ? startDate.AddDays(validityPeriod) : null,
            Type = faker.Random.String(),
            ValidityPeriod = validityPeriod,
            RequirePayment = withPayment
        };
        
        accountDocument.SubscriptionOrders.Add(subscription);
        
        return accountDocument;
    }
}