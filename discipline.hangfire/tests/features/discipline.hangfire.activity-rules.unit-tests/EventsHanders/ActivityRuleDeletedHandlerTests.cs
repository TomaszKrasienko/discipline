using discipline.hangfire.activity_rules.DAL.Repositories;
using discipline.hangfire.activity_rules.Events.External;
using discipline.hangfire.activity_rules.Events.External.Handlers;
using discipline.hangfire.activity_rules.Models;
using discipline.hangfire.shared.abstractions.Identifiers;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace discipline.hangfire.activity_rules.unit_tests.EventsHanders;

public class ActivityRuleDeletedHandlerTests
{
    private Task Act(ActivityRuleDeleted @event) => _handler.HandleAsync(@event, CancellationToken.None);
    
    [Fact]
    public async Task GivenExistingActivityRule_WhenHandleAsync_ThenRemoves()
    {
        // Arrange
        var activityRule = ActivityRule.Create(
            ActivityRuleId.New(),
            UserId.New(),
            "test_title",
            "test_mode",
            null);

        var @event = new ActivityRuleDeleted(
            activityRule.UserId.ToString(),
            activityRule.ActivityRuleId.ToString());

        _activityRuleRepository
            .GetByIdAsync(
                activityRule.ActivityRuleId,
                activityRule.UserId,
                CancellationToken.None)
            .Returns(activityRule);
        
        // Act
        await Act(@event);
        
        // Assert
        await _activityRuleRepository
            .Received(1)
            .DeleteAsync(
                Arg.Is<ActivityRule>(activityRule),
                CancellationToken.None);
    }

    [Fact]
    public async Task GivenNonExistingActivityRule_WhenHandleAsync_ThenNotRemoves()
    {
        // Arrange
        var activityRuleId = ActivityRuleId.New();
        var userId = UserId.New();
        
        var @event = new ActivityRuleDeleted(
            userId.ToString(),
            activityRuleId.ToString());
        
        _activityRuleRepository
            .GetByIdAsync(
                activityRuleId,
                userId,
                CancellationToken.None)
            .ReturnsNull();
        
        // Act
        await Act(@event);
        
        // Assert
        await _activityRuleRepository
            .Received(0)
            .DeleteAsync(
                Arg.Any<ActivityRule>(),
                CancellationToken.None);
    }
    
    private readonly IActivityRuleRepository _activityRuleRepository;
    private readonly ActivityRuleDeletedHandler _handler;

    public ActivityRuleDeletedHandlerTests()
    {
        var logger = Substitute.For<ILogger<ActivityRuleDeletedHandler>>();
        _activityRuleRepository = Substitute.For<IActivityRuleRepository>();
        _handler = new ActivityRuleDeletedHandler(
            logger,
            _activityRuleRepository);
    }
}