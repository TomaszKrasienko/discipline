using Bogus;
using discipline.centre.users.infrastructure.DAL.Users.Documents;

namespace discipline.centre.users.tests.sharedkernel.Infrastructure;

internal static class UserDocumentFactory
{
    internal static UserDocument Get()
        => Get(1).Single();
    
    private static List<UserDocument> Get(int count)
        => GetFaker().Generate(count);

    private static Faker<UserDocument> GetFaker()
        => new Faker<UserDocument>()
            .RuleFor(f => f.Id, Ulid.NewUlid().ToString())
            .RuleFor(f => f.Email, v => v.Internet.Email())
            .RuleFor(f => f.Password, v => "Test123!")
            .RuleFor(f => f.FirstName, v => v.Name.FirstName())
            .RuleFor(f => f.LastName, v => v.Name.LastName())
            .RuleFor(f => f.Status, v => "Created");
}