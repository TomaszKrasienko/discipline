using discipline.centre.users.application.Accounts.DTOs.Requests;
using Shouldly;
using Xunit;

namespace discipline.centre.users.application.unittests.Accounts.DTOs.Mappers;

public sealed class SignInRequestDtoMapperExtensionsTests
{
    [Fact]
    public void GivenSignInRequest_WhenToCommand_ThenReturnsSignInCommand()
    {
        // Arrange
        var request = new SignInRequestDto(
            "test@test.pl",
            "test123!");
        
        // Act
        var command = request.ToCommand();
        
        // Assert
        command.Email.ShouldBe(request.Email);
        command.Password.ShouldBe(request.Password);
    }
}