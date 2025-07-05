using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Accounts;
using discipline.centre.users.domain.Accounts.ValueObjects.SubscriptionOrder;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.centre.users.domain.unitTests.Accounts.ValueObjects;

public sealed class SubscriptionOrderTests
{
    #region Create

    [Fact]
    public void GivenSubscriptionRequirePayment_WhenCreateWithPayment_ThenReturnsSubscriptionOrder()
    {
        // Arrange
        var subscriptionOrderId = SubscriptionOrderId.New();
        
        var interval = Interval.Create(
            DateOnly.FromDateTime(DateTime.Now.AddDays(-30)),
            DateOnly.FromDateTime(DateTime.Now));
        
        var subscriptionDetails = SubscriptionDetails.Create(
            "test_type",
            3,
            true);
        
        _timeProvider
            .GetUtcNow()
            .Returns(DateTimeOffset.Now);
        var payment = Payment.Create(
            _timeProvider,
            123m);
        
        // Act
        var result = SubscriptionOrder.Create(
            subscriptionOrderId,
            interval,
            subscriptionDetails,
            payment);
        
        // Assert
        result.Interval.StartDate.ShouldBe(interval.StartDate);
        result.Interval.FinishDate.ShouldBe(interval.FinishDate);
        
        result.Subscription.RequirePayment.ShouldBe(subscriptionDetails.RequirePayment);
        result.Subscription.Type.ShouldBe(subscriptionDetails.Type);
        result.Subscription.ValidityPeriod.ShouldBe(subscriptionDetails.ValidityPeriod);
        
        result.Payment!.CreatedAt.ShouldBe(payment.CreatedAt);
        result.Payment!.Value.ShouldBe(payment.Value);
    }
    
    [Fact]
    public void GivenSubscriptionNotRequirePayment_WhenCreateWithoutPayment_ThenReturnsSubscriptionOrder()
    {
        // Arrange
        var subscriptionOrderId = SubscriptionOrderId.New();
        
        var interval = Interval.Create(
            DateOnly.FromDateTime(DateTime.Now.AddDays(-30)),
            DateOnly.FromDateTime(DateTime.Now));
        
        var subscriptionDetails = SubscriptionDetails.Create(
            "test_type",
            null,
            false);
        
        // Act
        var result = SubscriptionOrder.Create(
            subscriptionOrderId,
            interval,
            subscriptionDetails,
            null);
        
        // Assert
        result.Interval.StartDate.ShouldBe(interval.StartDate);
        result.Interval.FinishDate.ShouldBe(interval.FinishDate);
        
        result.Subscription.RequirePayment.ShouldBe(subscriptionDetails.RequirePayment);
        result.Subscription.Type.ShouldBe(subscriptionDetails.Type);
        result.Subscription.ValidityPeriod.ShouldBe(subscriptionDetails.ValidityPeriod);
        
        result.Payment.ShouldBeNull();
    }

    [Fact]
    public void GivenSubscriptionRequirePayment_WhenCreateWithoutPayment_ThenThrowsDomainExceptionWithCode_Account_SubscriptionOrder_RequirePayment()
    {
        // Arrange
        var subscriptionOrderId = SubscriptionOrderId.New();
        
        var interval = Interval.Create(
            DateOnly.FromDateTime(DateTime.Now.AddDays(-30)),
            DateOnly.FromDateTime(DateTime.Now));
        
        var subscriptionDetails = SubscriptionDetails.Create(
            "test_type",
            null,
            false);
        
        // Act
        var exception = Record.Exception(() => SubscriptionOrder.Create(
            subscriptionOrderId,
            interval,
            subscriptionDetails,
            null));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldContain("Account.SubscriptionOrder.RequirePayment");
    }
    
    [Fact]
    public void GivenSubscriptionNotRequirePayment_WhenCreateWithPayment_ThenThrowsDomainExceptionWithCode_Account_SubscriptionOrder_NotRequirePayment()
    {
        // Arrange
        var subscriptionOrderId = SubscriptionOrderId.New();
        
        var interval = Interval.Create(
            DateOnly.FromDateTime(DateTime.Now.AddDays(-30)),
            DateOnly.FromDateTime(DateTime.Now));
        
        var subscriptionDetails = SubscriptionDetails.Create(
            "test_type",
            3,
            true);
        
        _timeProvider
            .GetUtcNow()
            .Returns(DateTimeOffset.Now);
        var payment = Payment.Create(
            _timeProvider,
            123m);
        
        // Act
        var exception = Record.Exception(() => SubscriptionOrder.Create(
            subscriptionOrderId,
            interval,
            subscriptionDetails,
            payment));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldContain("Account.SubscriptionOrder.NotRequirePayment");
    }
    
    #endregion

    private readonly TimeProvider _timeProvider;

    public SubscriptionOrderTests()
    {
        _timeProvider = Substitute.For<TimeProvider>();
    }
}