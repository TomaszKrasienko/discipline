using discipline.hangfire.account_modification.DAL.Repositories;
using discipline.hangfire.account_modification.Events.External;
using discipline.hangfire.account_modification.Models;
using discipline.hangfire.account_modification.Strategies;
using discipline.hangfire.shared.abstractions.Identifiers;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using static NSubstitute.Arg;

namespace discipline.hangfire.account_modification.unit_tests.Strategies;

public sealed class AccountRegisteredStrategyTests
{
    [Fact]
    public async Task GivenNotExistingAccount_WhenHandleAsync_ShouldAddAccount()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var @event = new AccountModified(Ulid.NewUlid().ToString());
        
        _repository
            .DoesExistAsync(AccountId.Parse(@event.AccountId), cancellationToken)
            .Returns(false);
        
        // Act
        await _strategy.HandleAsync(@event, cancellationToken);
        
        // Assert
        await _repository
            .Received(1)
            .AddAsync(
                Is<Account>(x => x.AccountId == AccountId.Parse(@event.AccountId)),
                cancellationToken);
    }

    [Fact]
    public async Task GivenExistingAccount_WhenHandleAsync_ShouldNotAddAccountAndLogWarning()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var @event = new AccountModified(Ulid.NewUlid().ToString());
        
        _repository
            .DoesExistAsync(AccountId.Parse(@event.AccountId), cancellationToken)
            .Returns(true);
        
        // Act
        await _strategy.HandleAsync(@event, cancellationToken);
        
        // Assert
        await _repository
            .Received(0)
            .AddAsync(Any<Account>(), cancellationToken);
    }

    [Fact]
    public void GivenAccountRegistered_WhenCanBeApplied_ShouldReturnTrue()
    {
        // Act
        var result = _strategy.CanBeApplied("TestRegistered");
        
        // Assert
        result.ShouldBeTrue();
    }
    
    private readonly IAccountRepository _repository;
    private readonly AccountRegisteredStrategy _strategy; 
    
    public AccountRegisteredStrategyTests()
    {
        _repository = Substitute.For<IAccountRepository>();
        _strategy = new AccountRegisteredStrategy(
            Substitute.For<ILogger<AccountRegisteredStrategy>>(),
            _repository);
    }
}