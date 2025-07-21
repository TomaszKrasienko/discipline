using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Accounts;
using discipline.centre.users.domain.Accounts.Specifications.Account;
using discipline.centre.users.domain.Accounts.Specifications.SubscriptionOrder;
using discipline.centre.users.domain.Subscriptions.Enums;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.centre.users.domain.unitTests.Accounts.ValueObjects;

public sealed class AccountTests
{
    #region Create

    [Fact]
    public void GivenAllParametersWithoutPayment_WhenCreate_ShouldReturnAccountAndAddFreeSubscriptionOrder()
    {
        // Arrange
        var accountId = AccountId.New();
        const string login = "test_login";
        var passwordSpecification = new PasswordSpecification(
            "password",
            "hashed_password");
        var order = new SubscriptionOrderSpecification(
            SubscriptionId.New(), 
            "test_subscription_type",
            null,
            false,
            null);
        
        var now = DateTimeOffset.UtcNow;
        _timeProvider
            .GetUtcNow()
            .Returns(now);
        
        // Act
        var result = Account.Create(
            accountId,
            login,
            passwordSpecification,
            _timeProvider,
            order);
        
        // Assert
        result.Login.Value.ShouldBe(login);
        result.Password.Value.ShouldBe(passwordSpecification.HashedPassword);

        var existingOrder = result.Orders.Single();
        existingOrder.SubscriptionId.ShouldBe(order.SubscriptionId);
        existingOrder.Interval.StartDate.ShouldBe(DateOnly.FromDateTime(now.DateTime));    
        existingOrder.Interval.FinishDate.ShouldBeNull();
        existingOrder.Subscription.Type.ShouldBe(order.SubscriptionType);
        existingOrder.Subscription.RequirePayment.ShouldBe(order.RequirePayment);
        existingOrder.Subscription.ValidityPeriod.ShouldBeNull();
        existingOrder.Payment.ShouldBeNull();
    }
    
    [Fact]
    public void GivenAllParametersWithPaymentAndYearPeriod_WhenCreate_ThenReturnsAccountAndAddPaidSubscriptionOrderWithFinishDateAfterYear()
    {
        // Arrange
        var accountId = AccountId.New();
        const string login = "test_login";
        var passwordSpecification = new PasswordSpecification(
            "password",
            "hashed_password");
        var order = new SubscriptionOrderSpecification(
            SubscriptionId.New(),
            "test_subscription_type",
            Period.Year, 
            true,
            30.23m);
        
        var now = new DateTimeOffset(2025, 1, 1, 12, 0, 0, TimeSpan.Zero);
        _timeProvider
            .GetUtcNow()
            .Returns(now);
        
        // Act
        var result = Account.Create(
            accountId,
            login,
            passwordSpecification,
            _timeProvider,
            order);
        
        // Assert
        result.Login.Value.ShouldBe(login);
        result.Password.Value.ShouldBe(passwordSpecification.HashedPassword);

        var existingOrder = result.Orders.Single();
        existingOrder.SubscriptionId.ShouldBe(order.SubscriptionId);
        existingOrder.Interval.StartDate.ShouldBe(DateOnly.FromDateTime(now.DateTime));
        existingOrder.Interval.FinishDate.ShouldBe(DateOnly.FromDateTime(now.DateTime).AddYears(1));
        existingOrder.Subscription.Type.ShouldBe(order.SubscriptionType);
        existingOrder.Subscription.RequirePayment.ShouldBe(order.RequirePayment);
        existingOrder.Subscription.ValidityPeriod.ShouldBe(order.Period);
        existingOrder.Payment!.CreatedAt.ShouldBe(now);
        existingOrder.Payment!.Value.ShouldBe(order.PaymentValue!.Value);
    }
    
    [Fact]
    public void GivenAllParametersWithPaymentAndMonthPeriod_WhenCreate_ThenReturnsAccountAndAddPaidSubscriptionOrderWithFinishDateAfterMonth()
    {
        // Arrange
        var accountId = AccountId.New();
        const string login = "test_login";
        var passwordSpecification = new PasswordSpecification(
            "password",
            "hashed_password");
        var order = new SubscriptionOrderSpecification(
            SubscriptionId.New(),
            "test_subscription_type",
            Period.Month, 
            true,
            30.23m);
        
        var now = new DateTimeOffset(2025, 1, 1, 12, 0, 0, TimeSpan.Zero);
        _timeProvider
            .GetUtcNow()
            .Returns(now);
        
        // Act
        var result = Account.Create(
            accountId,
            login,
            passwordSpecification,
            _timeProvider,
            order);
        
        // Assert
        result.Login.Value.ShouldBe(login);
        result.Password.Value.ShouldBe(passwordSpecification.HashedPassword);

        var existingOrder = result.Orders.Single();
        existingOrder.SubscriptionId.ShouldBe(order.SubscriptionId);
        existingOrder.Interval.StartDate.ShouldBe(DateOnly.FromDateTime(now.DateTime));
        existingOrder.Interval.FinishDate.ShouldBe(DateOnly.FromDateTime(now.DateTime).AddMonths(1));
        existingOrder.Subscription.Type.ShouldBe(order.SubscriptionType);
        existingOrder.Subscription.RequirePayment.ShouldBe(order.RequirePayment);
        existingOrder.Subscription.ValidityPeriod.ShouldBe(order.Period);
        existingOrder.Payment!.CreatedAt.ShouldBe(now);
        existingOrder.Payment!.Value.ShouldBe(order.PaymentValue!.Value);
    }
    
    #endregion
    
    private readonly TimeProvider _timeProvider;

    public AccountTests()
    {
        _timeProvider = Substitute.For<TimeProvider>();
    }
}