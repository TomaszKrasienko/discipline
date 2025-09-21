using Bogus;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Users;
using discipline.centre.users.domain.Users.Specifications;

namespace discipline.centre.users.tests.shared_kernel.Domain;

public static class UserFakeDataFactory
{
    public static User Get()
    {
        var faker = new Faker<User>().CustomInstantiator(v 
            => User.Create(
                UserId.New(),
                v.Internet.Email(),
                new FullNameSpecification(v.Name.FirstName(), v.Name.LastName()),
                AccountId.New()));
        
        return faker.Generate();
    }
}