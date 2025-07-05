using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.users.domain.Accounts.ValueObjects.Account;
using Shouldly;
using Xunit;

namespace discipline.centre.users.domain.unitTests.Accounts.ValueObjects.Accounts;

public sealed class LoginTests
{
    #region Create
    [Fact]
    public void GivenNotEmptyLogin_WhenCreate_ThenReturnLogin()
    {
        // Arrange
        const string value = "test_login";
        
        // Act
        var login = Login.Create(value);
        
        // Assert
        login.Value.ShouldBe(value);
    }

    [Fact]
    public void GivenEmptyLogin_WhenCreate_ThenThrowDomainExceptionWithCode_Account_EmptyLogin()
    {
        // Act
        var exception = Record.Exception(() =>  Login.Create(string.Empty));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("Account.EmptyLogin");
    }
    #endregion
}