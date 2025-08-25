using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.application.Accounts.DTOs.Requests;
using discipline.centre.users.domain.Subscriptions.Enums;
using Shouldly;
using Xunit;

namespace discipline.centre.users.application.unittests.Accounts.DTOs.Mappers;

public sealed class SignUpRequestDtoMapperExtensionsTest
{
    [Fact]
    public void GivenSignUpRequestDtoAndAccountId_WhenToCommand_ThenReturnsSignUpCommand()
    {
        // Arrange
        var request = new SignUpRequestDto(
            "test@test.pl",
            "pass123!",
            SubscriptionId.New().Value.ToString(),
            Period.Month.Value,
            "first_name",
            "last_name",
            123);
        
        var accountId = AccountId.New();
        
        // Act
        var command = request.ToCommand(accountId);
        
        // Assert
        command.AccountId.ShouldBe(accountId);
        command.Email.ShouldBe(request.Email);
        command.Password.ShouldBe(request.Password);
        command.SubscriptionId.Value.ToString().ShouldBe(request.SubscriptionId);
        command.Period!.Value.Value.ShouldBe(request.Period);
        command.FirstName.ShouldBe(request.FirstName);
        command.LastName.ShouldBe(request.LastName);
        command.PaymentValue.ShouldBe(request.PaymentValue);
    }

    [Fact]
    public void GivenSignUpRequestWithInvalidSubscriptionId_WhenToCommand_ThenThrowsArgumentExceptionWithCode_SubscriptionId_InvalidFormat()
    {
        // Act
        var exception = Record.Exception(() => new SignUpRequestDto(
            "test@test.pl",
            "pass123!",
            Guid.NewGuid().ToString(),
            Period.Month.Value,
            "first_name",
            "last_name",
            123).ToCommand(AccountId.New()));
        
        // Assert
        exception.ShouldBeOfType<ArgumentException>();
        ((ArgumentException)exception).Message.ShouldBe("SubscriptionId.InvalidFormat");
    }
}