using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.users.domain.Subscriptions;
using discipline.centre.users.domain.Subscriptions.Policies;
using discipline.centre.users.domain.Subscriptions.Policies.Abstractions;
using discipline.centre.users.tests.sharedkernel.Infrastructure;
using Shouldly;

namespace discipline.centre.users.infrastructure.unittests.DAL.Subscriptions;

public sealed class SubscriptionDocumentsMapperExtensions
{
    [Fact]
    public void GivenValidSubscriptionDocumentWithPolicy_WhenToEntity_ThenReturnsSubscription()
    {
        // Arrange
        var document = SubscriptionDocumentFakeDataFactory
            .Get();

        var priceDocument = document.Prices.Single();
        
        // Act
        var entity = document.ToEntity(_policy);
        
        // Assert
        entity.Id.Value.ToString().ShouldBe(document.Id);
        entity.Type.Value.ShouldBe(document.Type);

        var price = entity.Prices.Single();
        price.PerMonth.ShouldBe(priceDocument.PerMonth);
        price.PerYear.ShouldBe(priceDocument.PerYear);
        price.Currency.Shorcut.ShouldBe(priceDocument.Currency);
    }

    [Fact]
    public void GivenInvalidId_WhenToEntity_ThenThrowsArgumentExceptionWithCode_SubscriptionId_InvalidFormat()
    {
        // Arrange
        var document = SubscriptionDocumentFakeDataFactory
            .Get();

        document.Id = Guid.NewGuid().ToString();
        
        // Act
        var exception = Record.Exception(() => document.ToEntity(_policy));
        
        // Assert
        exception.ShouldBeOfType<InvalidArgumentException>();
        ((InvalidArgumentException)exception).Code.ShouldBe("SubscriptionId.InvalidFormat");
    }

    [Fact]
    public void GivenInvalidType_WhenToEntity_ThenThrowsArgumentExceptionWithCode_SubscriptionType_InvalidFormat()
    {
        // Arrange
        var document = SubscriptionDocumentFakeDataFactory
            .Get();
        document.Type = "test_invalid_type";
        
        // Act
        var exception = Record.Exception(() => document.ToEntity(_policy));
        
        // Assert
        exception.ShouldBeOfType<InvalidArgumentException>();
        ((InvalidArgumentException)exception).Code.ShouldBe("SubscriptionType.InvalidFormat");
    }

    [Fact]
    public void GivenInvalidCurrency_WhenToEntity_ThenThrowsArgumentExceptionWithCode_SubscriptionType_InvalidFormat()
    {
        // Arrange
        var document = SubscriptionDocumentFakeDataFactory
            .Get();
        document.Prices.Single().Currency = "invalid_currency";
        
        // Act
        var exception = Record.Exception(() => document.ToEntity(_policy));
        
        // Assert
        exception.ShouldBeOfType<InvalidArgumentException>();
        ((InvalidArgumentException)exception).Code.ShouldBe("Currency.InvalidFormat");
    }
    
    #region Arrange
    private readonly ISubscriptionPolicy _policy = new PremiumSubscriptionPolicy();
    #endregion
}