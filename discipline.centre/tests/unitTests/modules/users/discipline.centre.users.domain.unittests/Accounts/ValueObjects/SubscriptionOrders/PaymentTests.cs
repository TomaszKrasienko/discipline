using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.users.domain.Accounts.ValueObjects.SubscriptionOrder;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.centre.users.domain.unittests.Accounts.ValueObjects.SubscriptionOrders;

public sealed class PaymentTests
{
    #region Create
    [Fact]
    public void GivenPositivePaymentValue_WhenCreate_ThenReturnPayment()
    {
         // Arrange
         const decimal value = 123.2m;
         var createdAt = DateTimeOffset.UtcNow;
         _timeProvider
             .GetUtcNow()
             .Returns(createdAt);
         
         // Act
         var result = Payment.Create(_timeProvider, value);
         
         // Assert
         result.CreatedAt.ShouldBe(createdAt);
         result.Value.ShouldBe(value);
    }

    [InlineData(0)]
    [InlineData(-1)]
    [Theory]
    public void GivenInvalidPaymentValue_WhenCreate_ThenThrowDomainExceptionWithCode_Account_SubscriptionOrder_PaymentValueBelowOrEqualZero(decimal value)
    {
        var createdAt = DateTimeOffset.UtcNow;
        _timeProvider
            .GetUtcNow()
            .Returns(createdAt);
         
        // Act
        var exception = Record.Exception(() => Payment.Create(_timeProvider, value));
         
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("Account.SubscriptionOrder.PaymentValueBelowOrEqualZero");
    }
    #endregion

    private readonly TimeProvider _timeProvider;

    public PaymentTests()
    {
        _timeProvider = Substitute.For<TimeProvider>();
    }
}