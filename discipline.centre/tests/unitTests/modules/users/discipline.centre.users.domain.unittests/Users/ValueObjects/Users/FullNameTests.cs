using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.users.domain.Users.ValueObjects;
using Shouldly;
using Xunit;

namespace discipline.centre.users.domain.unittests.Users.ValueObjects.Users;

public sealed class FullNameTests
{
    [Theory]
    [MemberData(nameof(GetValidFullName))]
    public void GivenValidFullName_QhenCreate_ThenReturnFullNameWithFirstNameAndLastName(string firstName,
        string lastName)
    {
        // Act
        var result = FullName.Create(firstName, lastName);
        
        // Assert
        result.FirstName.ShouldBe(firstName);
        result.LastName.ShouldBe(lastName);
    }

    public static IEnumerable<object[]> GetValidFullName()
    {
        yield return [new string('t', 2), new string('t', 2)];
        yield return [new string('t', 100), new string('t', 100)];
    }

    [Fact]
    public void GivenEmptyFirstName_WhenCreate_ThenThrowsDomainExceptionWithCode_User_FullName_EmptyFirstName()
    {
        // Act
        var exception = Record.Exception(() => FullName.Create(
            string.Empty,
            "last_name"));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("User.FullName.EmptyFirstName");
    }

    [Theory]
    [MemberData(nameof(GetInvalidFirstName))]
    public void GivenInvalidFirstName_WhenCreate_ThenThrowsDomainExceptionWithCode_User_FullName_FirstNameInvalidLength(string firstName)
    {
        // Act
        var exception = Record.Exception(() => FullName.Create(
            firstName,
            "last_name"));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("User.FullName.FirstNameInvalidLength");
    }

    [Fact]
    public void GivenEmptyLastName_WhenCreate_ThenThrowsDomainExceptionWithCode_User_FullName_EmptyLastName()
    {
        // Act
        var exception = Record.Exception(() => FullName.Create(
            "first_name",
            string.Empty));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("User.FullName.EmptyLastName");
    }

    public static IEnumerable<object[]> GetInvalidFirstName()
    {
        yield return [new string('t', 1)];
        yield return [new string('t', 101)];
    }
}