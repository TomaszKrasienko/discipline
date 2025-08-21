using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Users;
using discipline.centre.users.domain.Users.ValueObjects;
using discipline.centre.users.infrastructure.DAL.Users.Documents;

// ReSharper disable once CheckNamespace
namespace discipline.centre.users.infrastructure.DAL.Documents;


//TODO: Unit tests
internal static class UserDocumentsMapperExtensions
{
    internal static User ToEntity(this UserDocument document)
    {
        var id = UserId.Parse(document.Id);
        var fullName = FullName.Create(document.FirstName, document.LastName);
        var accountId = AccountId.Parse(document.AccountId);

        return new User(
            id,
            document.Email,
            fullName,
            accountId);
    }
}