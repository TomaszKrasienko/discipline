using discipline.centre.users.infrastructure.DAL.Subscriptions.Documents;
using discipline.centre.users.tests.sharedkernel.Domain;
using Shouldly;

namespace discipline.centre.users.infrastructure.unittests.DAL.Subscriptions;

public sealed class SubscriptionMapperExtensionsTests
{
    [Fact]
    public void GivenSubscriptionWithPrices_WhenToDocument_ThenReturnsSubscriptionDocument()
    {
        // Arrange
        var subscription = SubscriptionFakeDataFactory
            .GetPremium();

        var price = subscription.Prices.Single();
        
        // Act
        var document = subscription.ToDocument();
        
        // Assert
        document.Id.ShouldBe(subscription.Id.Value.ToString());
        document.Type.ShouldBe(subscription.Type.Value);

        var subscriptionPriceDocument = document.Prices.Single();
        subscriptionPriceDocument.PerMonth.ShouldBe(price.PerMonth);
        subscriptionPriceDocument.PerYear.ShouldBe(price.PerYear);
        subscriptionPriceDocument.Currency.ShouldBe(price.Currency.Shorcut);
    }
}