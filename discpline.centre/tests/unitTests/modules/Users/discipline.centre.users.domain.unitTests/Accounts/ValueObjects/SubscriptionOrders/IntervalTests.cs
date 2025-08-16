using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.users.domain.Accounts.ValueObjects.SubscriptionOrder;
using Shouldly;
using Xunit;

namespace discipline.centre.users.domain.unittests.Accounts.ValueObjects.SubscriptionOrders;

public sealed class IntervalTests
{
    #region Create
    [Fact]
    public void GivenStartDateBeforeFinishDate_WhenCreate_ThenReturnIntervalWithValues()
    {
        // Arrange
        var startDate = new DateOnly(2000, 1, 1);
        var plannedFinishDate = new DateOnly(2000, 12, 1);
        var finishDate = new DateOnly(2000, 12, 31);
        
        // Act
        var result = Interval.Create(
            startDate,
            plannedFinishDate,
            finishDate);
        
        // Assert
        result.StartDate.ShouldBe(startDate);
        result.PlannedFinishDate.ShouldBe(plannedFinishDate);
        result.FinishDate.ShouldBe(finishDate);
    }

    [Fact]
    public void GivenStartDateWithoutFinishDates_WhenCreate_ThenReturnIntervalWithValues()
    {
        // Arrange
        var startDate = new DateOnly(2000, 1, 1);
        
        // Act
        var result = Interval.Create(startDate, null, null);
        
        // Assert
        result.StartDate.ShouldBe(startDate);
        result.PlannedFinishDate.ShouldBeNull();
        result.FinishDate.ShouldBeNull();
    }
    
    [Fact]
    public void GivenPlannedFinishDateBeforeStartDate_WhenCreate_ThenThrowDomainExceptionWithCode()
    {
        // Arrange
        var startDate = new DateOnly(2001, 1, 1);
        var plannedFinishDate = new DateOnly(2000, 12, 31);
        
        // Act
        var exception = Record.Exception(() => Interval.Create(
            startDate,
            plannedFinishDate,
            null));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("Account.SubscriptionOrder.InvalidIntervalPlannedFinishDate");
    }

    [Fact]
    public void GivenFinishDateBeforeStartDate_WhenCreate_ThenThrowDomainExceptionWithCode()
    {
        // Arrange
        var startDate = new DateOnly(2001, 1, 1);
        var finishDate = new DateOnly(2000, 12, 31);
        
        // Act
        var exception = Record.Exception(() => Interval.Create(startDate, null, finishDate));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("Account.SubscriptionOrder.InvalidIntervalFinishDate");
    }
    #endregion
}