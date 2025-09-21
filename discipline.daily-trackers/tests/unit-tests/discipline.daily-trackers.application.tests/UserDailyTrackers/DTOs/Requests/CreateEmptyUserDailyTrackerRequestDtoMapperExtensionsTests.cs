using discipline.daily_trackers.application.UserDailyTrackers.DTOs.Requests;
using Shouldly;

namespace discipline.daily_trackers.application.tests.UserDailyTrackers.DTOs.Requests;

public sealed class CreateEmptyUserDailyTrackerRequestDtoMapperExtensionsTests
{
    [Fact]
    public void GivenCreateEmptyUserDailyTrackerRequestDto_WhenToCommand_ThenReturnsCreateEmptyUserDailyTrackerCommand()
    {
        // Arrange
        var request = new CreateEmptyUserDailyTrackerRequestDto(
            Ulid.NewUlid().ToString(),
            Ulid.NewUlid().ToString(),
            DateOnly.FromDateTime(DateTimeOffset.UtcNow.DateTime));
        
        // Act
        var result = request.ToCommand();
        
        // Assert
        result.Id.ToString().ShouldBe(request.DailyTrackerId);
        result.AccountId.ToString().ShouldBe(request.AccountId);
        result.Day.ShouldBe(request.Day);
    }
}