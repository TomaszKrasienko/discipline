using discipline.centre.activityrules.application.ActivityRules.Events;
using discipline.centre.activityrules.domain.Enums;
using discipline.centre.activityrules.domain.Events;
using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;
using ActivityRuleChanged = discipline.centre.activityrules.application.ActivityRules.Events.ActivityRuleChanged;

namespace discipline.centre.activityrules.application.unitTests.ActivityRules.Events;

public sealed class EventsMapExtensionsTests
{
    [Fact]
    public void GivenActivityRuleCreated_WhenMapAsIntegrationEvent_ThenShouldMapOnActivityRuleRegisteredEvent()
    {
        // Arrange
        const int selectedDay = 1;
        var domainEvent = new ActivityRuleCreated(
            ActivityRuleId.New(),
            AccountId.New(),
            Details.Create("test_title", "test_note"),
            SelectedMode.Create(RuleMode.Custom, [selectedDay]));
        
        // Act
        var @event = domainEvent.MapAsIntegrationEvent();
        
        // Assert
        ((ActivityRuleRegistered)@event).ActivityRuleId.ShouldBe(domainEvent.ActivityRuleId.ToString());
        ((ActivityRuleRegistered)@event).UserId.ShouldBe(domainEvent.AccountId.ToString());
        ((ActivityRuleRegistered)@event).Title.ShouldBe(domainEvent.Details.Title);
        ((ActivityRuleRegistered)@event).Note.ShouldBe(domainEvent.Details.Note);
        ((ActivityRuleRegistered)@event).Mode.ShouldBe(domainEvent.Mode.Mode.Value);
        ((ActivityRuleRegistered)@event).Days!.Single().ShouldBe(selectedDay);
    }

    [Fact]
    public void GivenActivityRuleModeChanged_WhenMapAsIntegrationEvent_ThenShouldMapOnActivityRuleModeChangedEvent()
    {
        // Arrange
        var domainEvent = new domain.Events.ActivityRuleChanged(
            ActivityRuleId.New(),
            AccountId.New(),
            Details.Create("test_title", "test_note"),
            SelectedMode.Create(RuleMode.EveryDay, null));
        
        // Act
        var @event = domainEvent.MapAsIntegrationEvent();
        
        // Assert
        ((ActivityRuleChanged)@event).ActivityRuleId.ShouldBe(domainEvent.ActivityRuleId.ToString());
        ((ActivityRuleChanged)@event).UserId.ShouldBe(domainEvent.AccountId.ToString());
        ((ActivityRuleChanged)@event).Mode.ShouldBe(domainEvent.Mode.Mode.Value);
        ((ActivityRuleChanged)@event).Days.ShouldBeNull();
    }
    
    [Fact]
    public void MapAsIntegrationEvent_GivenNotExistingEvent_ShouldThrowInvalidOperationException()
    {
        //arrange
        var domainEvent = new TestEvent();
        
        //act
        var exception = Record.Exception(() => domainEvent.MapAsIntegrationEvent());
        
        //assert
        exception.ShouldBeOfType<InvalidOperationException>();
    }
}

public sealed record TestEvent : DomainEvent;