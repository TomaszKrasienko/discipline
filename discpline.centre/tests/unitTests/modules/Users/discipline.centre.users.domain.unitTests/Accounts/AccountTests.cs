using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Accounts;
using discipline.centre.users.domain.Accounts.Specifications.Account;
using discipline.centre.users.domain.Accounts.Specifications.SubscriptionOrder;
using discipline.centre.users.domain.Subscriptions.Enums;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.centre.users.domain.unittests.Accounts;

public sealed class AccountTests
{
    #region Create

    [Fact]
    public void GivenArgumentsWithNotPaymentRequireSubscription_WhenCreate_ShouldReturnAccountWithActiveOrderWithNullFinishDate()
    {
        // Arrange
        var accountId = AccountId.New();
        const string login = "test_login";
        var passwordSpecification = new PasswordSpecification(
            "test_password",
            Guid.NewGuid().ToString());
        var now = DateTimeOffset.UtcNow;
        _timeProvider
            .GetUtcNow()
            .Returns(now);

        var order = new SubscriptionOrderSpecification(
            SubscriptionId.New(),
            "test_type",
            Period.Month,
            false,
            null);
        
        // Act
        var result = Account.Create(
            accountId,
            login,
            passwordSpecification,
            _timeProvider,
            order);
        
        // Assert
        result.Id.ShouldBe(accountId);
        result.Login.ShouldBe(login);
        result.Password.Value.ShouldBe(passwordSpecification.HashedPassword);
        
        result.ActiveSubscriptionOrder!.SubscriptionId.ShouldBe(order.SubscriptionId);
        result.ActiveSubscriptionOrder!.Interval.StartDate.ShouldBe(DateOnly.FromDateTime(now.DateTime));
        result.ActiveSubscriptionOrder!.Interval.FinishDate.ShouldBeNull();
        result.ActiveSubscriptionOrder!.Payment.ShouldBeNull();
        result.ActiveSubscriptionOrder!.Subscription.RequirePayment.ShouldBe(order.RequirePayment);
        result.ActiveSubscriptionOrder!.Subscription.Type.ShouldBe(order.SubscriptionType);
        result.ActiveSubscriptionOrder!.Subscription.ValidityPeriod.ShouldBeNull();
    }
    
    #endregion

    private readonly TimeProvider _timeProvider;

    public AccountTests()
    {
        _timeProvider = Substitute.For<TimeProvider>();
    }
}