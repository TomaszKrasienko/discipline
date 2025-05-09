using discipline.centre.calendar.domain;
using discipline.centre.calendar.infrastructure.DAL.Documents;
using discipline.centre.calendar.tests.sharedkernel.Domain;
using Shouldly;

namespace discipline.centre.calendar.infrastructure.unitTests.Documents.Mappers;

public sealed class UserCalendarDayMapperExtensionsTests
{
    [Fact]
    public void GivenUserCalendarDayWithBothEvents_WhenAsDocument_ThenMapToUserCalendarDayDocument()
    {
        // Arrange
        var userCalendarDay = UserCalendarDayFakeDataFactory
            .GetWithImportantDate(true)
            .AddTimeEvent(true, true);
        
        var importantDateEvent = (ImportantDateEvent)userCalendarDay.Events.Single(x => x is ImportantDateEvent);
        
        var timeEvent = (TimeEvent)userCalendarDay.Events.Single(x => x is TimeEvent);
        
        // Act
        var result = userCalendarDay.AsDocument();
        
        // Assert
        result.UserCalendarId.ShouldBe(userCalendarDay.Id.ToString());
        result.Day.ShouldBe(userCalendarDay.Day.Value);
        result.UserId.ShouldBe(userCalendarDay.UserId.ToString());

        var resultImportantDate = result.Events.Single(x => x is ImportantDateEventDocument);
        resultImportantDate.EventId.ShouldBe(importantDateEvent.Id.ToString());
        resultImportantDate.Content.Title.ShouldBe(importantDateEvent.Content.Title);
        resultImportantDate.Content.Description.ShouldBe(importantDateEvent.Content.Description);
        
        var resultTimeEvent = (TimeEventDocument)result.Events.Single(x => x is TimeEventDocument);
        resultTimeEvent.EventId.ShouldBe(timeEvent.Id.ToString());
        resultTimeEvent.Content.Title.ShouldBe(timeEvent.Content.Title);
        resultTimeEvent.Content.Description.ShouldBe(timeEvent.Content.Description);
        resultTimeEvent.TimeSpan.From.ShouldBe(timeEvent.TimeSpan.From);
        resultTimeEvent.TimeSpan.To.ShouldBe(timeEvent.TimeSpan.To);
    }
}