using Bogus;
using discipline.centre.users.infrastructure.DAL.Users.Documents;

namespace discipline.centre.users.tests.shared_kernel.Infrastructure;

internal static class UserDocumentFactory
{
    internal static UserDocument Get()
    {
        var faker = new Faker<UserDocument>()
            .RuleFor(f => f.Id, Ulid.NewUlid().ToString())
            .RuleFor(f => f.Email, v => v.Internet.Email())
            .RuleFor(f => f.FirstName, v => v.Name.FirstName())
            .RuleFor(f => f.LastName, v => v.Name.LastName())
            .RuleFor(f => f.AccountId, v => Ulid.NewUlid().ToString());
       
       return faker.Generate();
    }
}