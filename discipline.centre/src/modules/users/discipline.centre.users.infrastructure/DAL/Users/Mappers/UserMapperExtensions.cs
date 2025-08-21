using discipline.centre.users.infrastructure.DAL.Users.Documents;

// ReSharper disable once CheckNamespace
namespace discipline.centre.users.domain.Users;

//TODO: Unit tests
internal static class UserMapperExtensions
{
    internal static UserDocument ToDocumet(this User entity)
        => new ()
        {
            Id = entity.Id.Value.ToString(),
            Email = entity.Email.Value,
            FirstName = entity.FullName.FirstName,
            LastName = entity.FullName.LastName,
            AccountId = entity.AccountId.ToString()
        };
}