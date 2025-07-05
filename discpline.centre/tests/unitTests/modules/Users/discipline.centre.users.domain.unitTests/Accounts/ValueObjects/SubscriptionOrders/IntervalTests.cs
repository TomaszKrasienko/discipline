using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.users.domain.Accounts.ValueObjects.SubscriptionOrder;
using Shouldly;
using Xunit;

namespace discipline.centre.users.domain.unitTests.Accounts.ValueObjects.SubscriptionOrders;

public sealed class IntervalTests
{
    #region Create
    [Fact]
    public void GivenStartDateBeforeFinishDate_WhenCreate_ThenReturnIntervalWithValues()
    {
        // Arrange
        var startDate = new DateOnly(2000, 1, 1);
        var finishDate = new DateOnly(2000, 12, 31);
        
        // Act
        var result = Interval.Create(startDate, finishDate);
        
        // Assert
        result.StartDate.ShouldBe(startDate);
        result.FinishDate.ShouldBe(finishDate);
    }

    [Fact]
    public void GivenStartDateWithoutEndDate_WhenCreate_ThenReturnIntervalWithValues()
    {
        // Arrange
        var startDate = new DateOnly(2000, 1, 1);
        
        // Act
        var result = Interval.Create(startDate, null);
        
        // Assert
        result.StartDate.ShouldBe(startDate);
        result.FinishDate.ShouldBeNull();
    }

    [Fact]
    public void GivenEndDateBeforeStartDate_WhenCreate_ThenThrowDomainExceptionWithCode()
    {
        // Arrange
        var startDate = new DateOnly(2001, 1, 1);
        var finishDate = new DateOnly(2000, 12, 31);
        
        // Act
        var exception = Record.Exception(() => Interval.Create(startDate, finishDate));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("Account.SubscriptionOrder.InvalidIntervalFinishDate");
    }
    #endregion
}