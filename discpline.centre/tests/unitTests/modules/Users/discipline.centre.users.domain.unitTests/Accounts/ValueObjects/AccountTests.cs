using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Accounts;
using discipline.centre.users.domain.Accounts.Specifications.Account;
using discipline.centre.users.domain.Accounts.Specifications.SubscriptionOrder;
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
            DateOnly.FromDateTime(DateTime.UtcNow),
            "test_subscription_type",
            null,
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
        result.Login.Value.ShouldBe(login);
        result.Password.Value.ShouldBe(passwordSpecification.HashedPassword);

        var order = result.Orders.Single();
        order.Interval.ShouldBe(order.Interval);    
    }
    
    #endregion
    
    private readonly TimeProvider _timeProvider;

    public AccountTests()
    {
        _timeProvider = Substitute.For<TimeProvider>();
    }
}