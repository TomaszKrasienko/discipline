using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.users.domain.Accounts.ValueObjects.Account;
using Shouldly;
using Xunit;

namespace discipline.centre.users.domain.unitTests.Accounts.ValueObjects.Accounts;

public sealed class PasswordTests
{
    #region Create
    [Fact]
    public void GivenNotEmptyPassword_WhenCreate_ThenReturnPasswordWithHashedPassword()
    {
        // Arrange
        const string value = "pass";
        const string hashedValue = "hashedPassword";
        
        // Act
        var result = Password.Create(value, hashedValue);
        
        // Assert
        result.Value.ShouldBe(hashedValue);
    }

    [Fact]
    public void GivenEmptyPassword_WhenCreate_ThenThrowDomainExceptionWithCode_Account_PasswordTooWeak()
    {
        // Act
        var exception = Record.Exception(() => Password.Create(string.Empty, "hashedPassword"));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Message.ShouldContain("Account.PasswordTooWeak");
    }
    #endregion
}