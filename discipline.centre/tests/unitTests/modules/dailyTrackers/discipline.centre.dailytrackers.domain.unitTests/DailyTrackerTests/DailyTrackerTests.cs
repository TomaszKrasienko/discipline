using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.domain.unitTests.DailyTrackerTests;

public sealed class DailyTrackerTests
{
    #region Create

    [Fact]
    public void GivenValidParameters_WhenCreate_ShouldReturnDailyTracker()
    {
        // Arrange 
        var dailyTrackerId = DailyTrackerId.New();
        var day = DateOnly.FromDateTime(DateTime.Now);
        var accountId = AccountId.New();
        
        // Act
        var result = DailyTracker.Create(dailyTrackerId, day, accountId);
        
        // Assert
        result.Id.ShouldBe(dailyTrackerId);
        result.Day.Value.ShouldBe(day);
        result.AccountId.ShouldBe(accountId);
    }

    [Fact]
    public void GivenDefaultDateOnly_WhenCreate_ShouldThrowDomainExceptionWithCode_DailyTracker_Day_Default()
    {
        // Act
        var exception = Record.Exception(() =>
            DailyTracker.Create(
                DailyTrackerId.New(),
                default,
                AccountId.New()));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("DailyTracker.Day.Default");
    }

    #endregion
}