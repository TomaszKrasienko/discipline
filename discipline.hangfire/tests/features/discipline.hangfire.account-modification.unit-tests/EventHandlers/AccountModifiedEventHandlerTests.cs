using discipline.hangfire.account_modification.DAL.Repositories;
using discipline.hangfire.account_modification.Events.External;
using discipline.hangfire.account_modification.Events.External.Handlers;
using discipline.hangfire.account_modification.Models;
using discipline.hangfire.account_modification.Strategies;
using discipline.hangfire.account_modification.Strategies.Abstractions;
using discipline.hangfire.shared.abstractions.Identifiers;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace discipline.hangfire.account_modification.unit_tests.EventHandlers;

public sealed class AccountModifiedEventHandlerTests
{
    [Fact]
    public async Task GivenRegisteredEvent_WhenHandleAsync_ShouldAddAccount()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var accountId = AccountId.New();
        var @event = new AccountModified(accountId.ToString());
        const string messageType = "TestRegistered";
        
        _repository
            .DoesExistAsync(AccountId.Parse(@event.AccountId), cancellationToken)
            .Returns(false);
        
        // Act
        await _handler.HandleAsync(
            @event,
            cancellationToken,
            messageType);
        
        // Assert
        await _repository
            .Received(1)
            .AddAsync(Arg.Any<Account>(), cancellationToken);
    }
    
    [Fact]
    public async Task GivenRegisteredEvent_WhenHandleAsync_ShouldAddAccount()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var accountId = AccountId.New();
        var @event = new AccountModified(accountId.ToString());
        const string messageType = "TestRegistered";
        
        _repository
            .DoesExistAsync(AccountId.Parse(@event.AccountId), cancellationToken)
            .Returns(false);
        
        // Act
        await _handler.HandleAsync(
            @event,
            cancellationToken,
            messageType);
        
        // Assert
        await _repository
            .Received(1)
            .AddAsync(Arg.Any<Account>(), cancellationToken);
    }
    
    private readonly IAccountRepository _repository;
    private readonly IEnumerable<IAccountHandlingStrategy> _strategies;
    private readonly AccountModifiedEventHandler _handler;

    public AccountModifiedEventHandlerTests()
    {
        _repository = Substitute.For<IAccountRepository>();
        _strategies =
        [
            new AccountRegisteredStrategy(Substitute.For<ILogger<AccountRegisteredStrategy>>(), _repository)
        ];
        
        _handler = new AccountModifiedEventHandler(
            Substitute.For<ILogger<AccountModifiedEventHandler>>(),
            _strategies);
    }
}