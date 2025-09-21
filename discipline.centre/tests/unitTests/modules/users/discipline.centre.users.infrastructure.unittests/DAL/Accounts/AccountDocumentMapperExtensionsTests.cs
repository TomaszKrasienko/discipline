using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.infrastructure.DAL.Accounts.Documents;
using discipline.centre.users.tests.shared_kernel.Infrastructure;
using Shouldly;

namespace discipline.centre.users.infrastructure.unit_tests.DAL.Accounts;

public sealed class AccountDocumentMapperExtensionsTests
{
    [Fact]
    public void GivenAccountDocumentWithSubscriptionOrder_WhenToEntity_ThenReturnsAccount()
    {
        // Arrange
        var accountDocument = AccountDocumentFakeDataFactory
            .Get()
            .WithSubscriptionOrder(true);

        // Act
        var entity = accountDocument.ToEntity();
        
        // Assert
        entity.Id.ShouldBe(AccountId.Parse(accountDocument.Id));
        entity.Login.ShouldBe(accountDocument.Login);
        entity.Password.Value.ShouldBe(accountDocument.HashedPassword);
        entity.Orders.Count.ShouldBe(accountDocument.SubscriptionOrders.Count);
        
        var orderDocument = accountDocument.SubscriptionOrders.Single();
        var order = entity.Orders.Single();
        
        order.Id.ShouldBe(SubscriptionOrderId.Parse(orderDocument.Id));
        order.Interval.StartDate.ShouldBe(orderDocument.Interval.StartDate);
        order.Interval.PlannedFinishDate.ShouldBe(orderDocument.Interval.PlanedFinishDate);
        order.Interval.FinishDate.ShouldBe(orderDocument.Interval.FinishDate);
        order.Subscription.RequirePayment.ShouldBe(orderDocument.SubscriptionDetails.RequirePayment);
        order.Subscription.Type.ShouldBe(orderDocument.SubscriptionDetails.Type);
        order.Subscription.ValidityPeriod!.Value.Value.ShouldBe(orderDocument.SubscriptionDetails.ValidityPeriod);
        order.Payment!.CreatedAt.ShouldBe(orderDocument.Payment!.CreatedAt);
        order.Payment!.Value.ShouldBe(orderDocument.Payment.Value);
    }

    [Fact]
    public void GivenAccountDocumentWithoutSubscriptionOrder_WhenToEntity_ThenNotThrowsNullReferenceException()
    {
        // Arrange
        var accountDocument = AccountDocumentFakeDataFactory
            .Get();
        
        // Act
        var exception = Record.Exception(() => accountDocument.ToEntity());
        
        // Assert
        exception.ShouldBeNull();
    }
}