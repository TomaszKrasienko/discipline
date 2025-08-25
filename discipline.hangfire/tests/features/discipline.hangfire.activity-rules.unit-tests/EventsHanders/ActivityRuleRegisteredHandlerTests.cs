using discipline.hangfire.activity_rules.DAL.Repositories;
using discipline.hangfire.activity_rules.Events.External;
using discipline.hangfire.activity_rules.Events.External.Handlers;
using discipline.hangfire.activity_rules.Models;
using discipline.hangfire.shared.abstractions.Identifiers;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace discipline.hangfire.activity_rules.unit_tests.EventsHanders;

public sealed class ActivityRuleRegisteredHandlerTests
{
    private Task Act(ActivityRuleRegistered @event) => _handler.HandleAsync(@event, CancellationToken.None);
    
    [Fact]
    public async Task GivenNotExistingActivityRule_WhenHandleAsync_ThenShouldSaveActivityRule()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var stronglyActivityRuleId = ActivityRuleId.New();
        var stronglyUserId = UserId.New();
        List<int> days = [1];

        var @event = new ActivityRuleRegistered(
            stronglyActivityRuleId.ToString(),
            stronglyUserId.ToString(),
            "test_title",
            "test_mode",
            days);
        
        
        _repository
            .DoesActivityRuleExistAsync(
                stronglyActivityRuleId,
                stronglyUserId,
                cancellationToken)
            .Returns(false);
        
        // Act
        await Act(@event);
        
        // Assert
        await _repository
            .Received(1)
            .AddAsync(Arg.Is<ActivityRule>(arg
                => arg.ActivityRuleId.Value.ToString() == @event.ActivityRuleId &&
                   arg.UserId.Value.ToString() == @event.UserId &&
                   arg.Title == @event.Title &&
                   arg.Mode == @event.Mode &&
                   arg.SelectedDays == days), cancellationToken);
    }

    [Fact]
    public async Task GivenAlreadyExistingActivityRuleForUser_WhenHandleAsync_ThenShouldNotSaveActivityRule()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        
        var stronglyActivityRuleId = ActivityRuleId.New();
        var stronglyUserId = UserId.New();
        var @event = new ActivityRuleRegistered(
            stronglyActivityRuleId.ToString(),
            stronglyUserId.ToString(),
            "test_title",
            "test_mode",
            null);

        _repository
            .DoesActivityRuleExistAsync(
                stronglyActivityRuleId,
                stronglyUserId,
                cancellationToken)
            .Returns(true);
        
        // Act
        await Act(@event);
        
        // Assert
        await _repository
            .Received(0)
            .AddAsync(Arg.Any<ActivityRule>(), cancellationToken);
    }
    
    #region Arrange
    private readonly IActivityRuleRepository _repository;
    private readonly ActivityRuleRegisteredHandler _handler;
    
    public ActivityRuleRegisteredHandlerTests()
    {
        var logger = Substitute.For<ILogger<ActivityRuleRegisteredHandler>>();
        _repository = Substitute.For<IActivityRuleRepository>();
        _handler = new ActivityRuleRegisteredHandler(
            logger,
            _repository);
    }
    #endregion
}