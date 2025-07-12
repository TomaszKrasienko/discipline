using discipline.centre.shared.abstractions.SharedKernel.Aggregate;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Users.Events;
using discipline.centre.users.domain.Users.Specifications;
using discipline.centre.users.domain.Users.ValueObjects;

namespace discipline.centre.users.domain.Users;

public sealed class User : AggregateRoot<UserId, Ulid>
{
    public Email Email { get; }
    public FullName FullName { get; }
    public AccountId AccountId { get; }

    private User(
        UserId id,
        Email email,
        FullName fullName,
        AccountId accountId) : base(id)
    {
        Email = email;
        FullName = fullName;
        AccountId = accountId;
    }

    public static User Create(
        UserId id,
        string email,
        FullNameSpecification fullName,
        AccountId accountId)
    {
        var user = new User(
            id, 
            email,  
            FullName.Create(
                fullName.FirstName,
                fullName.LastName), 
            accountId);
        
        var @event = new UserCreated(id, email);
        user.AddDomainEvent(@event);
        return user;
    }
}