using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Subscriptions;
using discipline.centre.users.domain.Subscriptions.Enums;
using discipline.centre.users.domain.Subscriptions.Policies;
using discipline.centre.users.domain.Subscriptions.Policies.Abstractions;
using discipline.centre.users.domain.Subscriptions.Specifications;
using Shouldly;
using Xunit;

namespace discipline.centre.users.domain.unittests.Subscriptions;

public sealed class SubscriptionTests
{
    #region Create
    
    [Fact]
    public void GivenStandardSubscriptionTypeParameters_WhenCreate_ThenReturnsSubscriptionWithStandardTypePolicy()
    {
        // Arrange
        var id = SubscriptionId.New();
        
        // Act
        var result = Subscription.Create(
            id, 
            SubscriptionType.Standard,
            [],
            _policies
            );
        
        // Assert
        result.Id.ShouldBe(id);
        result.GetAllowedNumberOfDailyTasks().HasValue.ShouldBeTrue();
        result.GetAllowedNumberOfDailyTasks()!.Value.ShouldBe(10);
        result.GetAllowedNumberOfRules().HasValue.ShouldBeTrue();
        result.GetAllowedNumberOfRules()!.Value.ShouldBe(5);
    }

    [Fact]
    public void GivenPremiumSubscriptionTypeParameters_WhenCreate_ThenReturnsSubscriptionWithPremiumTypePolicy()
    {
        // Arrange
        var id = SubscriptionId.New();
        
        // Act
        var result = Subscription.Create(
            id, 
            SubscriptionType.Premium,
            [new PriceSpecification(10m, 100m, Currency.Pln)],
            _policies
        );
        
        // Assert
        result.Id.ShouldBe(id);
        result.GetAllowedNumberOfDailyTasks().HasValue.ShouldBeFalse();
        result.GetAllowedNumberOfRules().HasValue.ShouldBeFalse();
    }
    
    [Fact]
    public void GivenAdminSubscriptionTypeParameters_WhenCreate_ThenReturnsSubscriptionWithAdminTypePolicy()
    {
        // Arrange
        var id = SubscriptionId.New();
        
        // Act
        var result = Subscription.Create(
            id, 
            SubscriptionType.Admin,
            [],
            _policies
        );
        
        // Assert
        result.Id.ShouldBe(id);
        result.GetAllowedNumberOfDailyTasks().HasValue.ShouldBeFalse();
        result.GetAllowedNumberOfRules().HasValue.ShouldBeFalse();
    }

    [Fact]
    public void GivenPremiumSubscriptionTypeParameters_WhenCreate_ThenReturnsSubscriptionWithListOfPrices()
    {
        // Arrange
        var id = SubscriptionId.New();
        var price = new PriceSpecification(10m, 100m, Currency.Pln);
        
        // Act
        var result = Subscription.Create(
            id,
            SubscriptionType.Premium, 
            [price],
            _policies);

        // Arrange
        var currentPrice = result.Prices.Single();
        currentPrice.PerMonth.ShouldBe(price.PerMonth);
        currentPrice.PerYear.ShouldBe(price.PerYear);
        currentPrice.Currency.ShouldBe(price.Currency);
    }

    [Fact]
    public void GivenStandardSubscriptionParameters_WhenCreateWithPrices_ThenThrowsDomainExceptionWithCode_Subscription_PaymentNotRequired()
    {
        // Act
        var exception = Record.Exception(() => Subscription.Create(
            SubscriptionId.New(),
            SubscriptionType.Standard,
            [new PriceSpecification(10m, 100m, Currency.Pln)],
            _policies));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("Subscription.PaymentNotRequired");
    }
    
    [Fact]
    public void GivenAdminSubscriptionParameters_WhenCreateWithPrices_ThenThrowsDomainExceptionWithCode_Subscription_PaymentNotRequired()
    {
        // Act
        var exception = Record.Exception(() => Subscription.Create(
            SubscriptionId.New(),
            SubscriptionType.Admin,
            [new PriceSpecification(10m, 100m, Currency.Pln)],
            _policies));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("Subscription.PaymentNotRequired");
    }
    
    [Fact]
    public void GivenPremiumSubscriptionParameters_WhenCreateWithoutPrices_ThenThrowsDomainExceptionWithCode_Subscription_RequiredPayment()
    {
        // Act
        var exception = Record.Exception(() => Subscription.Create(
            SubscriptionId.New(),
            SubscriptionType.Premium,
            [],
            _policies));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("Subscription.RequiredPayment");
    }
    
    #endregion
    
    private readonly IEnumerable<ISubscriptionPolicy> _policies =
    [
        new AdminSubscriptionPolicy(),
        new PremiumSubscriptionPolicy(),
        new StandardSubscriptionPolicy()
    ];
}