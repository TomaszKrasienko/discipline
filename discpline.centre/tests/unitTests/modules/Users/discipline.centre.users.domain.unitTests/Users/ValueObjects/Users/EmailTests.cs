using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.users.domain.Users.ValueObjects;
using Shouldly;
using Xunit;

namespace discipline.centre.users.domain.unitTests.Users.ValueObjects.Users;

public sealed class EmailTests
{
    [Fact]
    public void GivenValidEmail_WhenCreate_ThenReturnsEmailWithValue()
    {
        // Arrange
        var value = "test@test.pl";
        
        // Act
        var result = Email.Create(value);
        
        // Assert
        result.Value.ShouldBe(value);
    }

    [Fact]
    public void GivenEmptyEmail_WhenCreate_ThenThrowsDomainExceptionWithCode_User_EmptyEmail()
    {
        // Act
        var exception = Record.Exception(() => Email.Create(string.Empty));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("User.EmptyEmail");
    }
    
    [Fact]
    public void GivenInvalidEmail_WhenCreate_ThenThrowsDomainExceptionWithCode_User_InvalidEmail()
    {
        // Act
        var exception = Record.Exception(() => Email.Create(string.Empty));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("User.InvalidEmail");
    }
}