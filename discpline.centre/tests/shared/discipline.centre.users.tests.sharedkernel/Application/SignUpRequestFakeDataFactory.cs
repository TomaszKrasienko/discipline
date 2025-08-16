using Bogus;
using discipline.centre.users.application.Accounts.DTOs.Requests;
using discipline.centre.users.domain.Subscriptions.Enums;

namespace discipline.centre.users.tests.sharedkernel.Application;

public static class SignUpRequestFakeDataFactory
{
    public static SignUpRequestDto Get(bool withPayment = false)
    {
        var faker = new Faker<SignUpRequestDto>()
            .CustomInstantiator(v => new SignUpRequestDto(
                v.Internet.Email(),
                "Test123!",
                Ulid.NewUlid().ToString(),
                v.PickRandom<Period>(Period.GetAvailable()).Value,
                v.Name.FirstName(),
                v.Name.LastName(),
                withPayment
                    ? v.Random.Decimal(min: 1)
                    : null));
        return faker.Generate();
    }
}