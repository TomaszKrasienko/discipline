using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Users;
using discipline.centre.users.domain.Users.Events;
using discipline.centre.users.domain.Users.Specifications;
using Shouldly;
using Xunit;

namespace discipline.centre.users.domain.unittests.Users;

public sealed class UsersTests
{
    #region Create

    [Fact]
    public void GivenValidParameters_WhenCreate_ThenReturnsUser()
    {
        // Arrange
        var userId = UserId.New();
        const string email = "test@test.pl";
        var fullName = new FullNameSpecification(
            "first_name",
            "last_name");
        var accountId = AccountId.New();
        
        // Act
        var result = User.Create(
            userId,
            email,
            fullName,
            accountId);
        
        // Assert
        result.Id.ShouldBe(userId);
        result.Email.ShouldBe(email);
        result.FullName.FirstName.ShouldBe(fullName.FirstName);
        result.FullName.LastName.ShouldBe(fullName.LastName);
        result.AccountId.ShouldBe(accountId);
    }
    
    [Fact]
    public void GivenValidParameters_WhenCreate_ThenCreatesDomainEventOfUserCreatedType()
    {
        // Arrange
        var userId = UserId.New();
        const string email = "test@test.pl";
        var fullName = new FullNameSpecification(
            "first_name",
            "last_name");
        var accountId = AccountId.New();
        
        // Act
        var result = User.Create(
            userId,
            email,
            fullName,
            accountId);
        
        // Assert
        result.DomainEvents.Any(x => x is UserCreated).ShouldBeTrue();
    }
    
    #endregion
}