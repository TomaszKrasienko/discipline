using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.application.Accounts.Commands;
using discipline.centre.users.application.Accounts.DTOs;
using discipline.centre.users.application.Accounts.Services;
using discipline.centre.users.domain.Accounts.Repositories;
using discipline.centre.users.domain.Subscriptions.Repositories;
using discipline.centre.users.tests.shared_kernel.Domain;
using discipline.libs.cqrs.abstractions.Commands;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Shouldly;
using Xunit;

namespace discipline.centre.users.application.unittests.Accounts.Commands;

public sealed class RefreshCommandHandlerTests
{
    private Task Act(RefreshCommand command) => _handler.HandleAsync(command, CancellationToken.None);
    
    [Fact]
    public async Task GivenExistingRefreshToken_WheHandleAsync_ThenSavesTokensByTokenStorage()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var account = AccountFakeDataFactory
            .Get()
            .WithSubscriptionOrder();
        var subscription = SubscriptionFakeDataFactory
            .GetStandard();
        var token = Guid.NewGuid().ToString();
        var refreshToken = Guid.NewGuid().ToString();
        
        var command = new RefreshCommand(
            account.Id, 
            Guid.NewGuid().ToString());

        _refreshTokenManager
            .DoesRefreshTokenExistAsync(command.RefreshToken, command.AccountId, cancellationToken)
            .Returns(true);
        
        _readAccountRepository
            .GetByIdAsync(command.AccountId, cancellationToken)
            .Returns(account);
        
        _readSubscriptionRepository
            .GetByIdAsync(account.ActiveSubscriptionOrder!.SubscriptionId, cancellationToken)
            .Returns(subscription);

        _authenticator
            .CreateToken(
                account.Id,
                subscription.Type.HasExpiryDate,
                account.ActiveSubscriptionOrder.Interval.FinishDate,
                subscription.GetAllowedNumberOfDailyTasks(),
                subscription.GetAllowedNumberOfRules())
            .Returns(token);
        
        _refreshTokenManager
            .GenerateAndReplaceAsync(account.Id, cancellationToken)
            .Returns(refreshToken);
        
        // Act
        await Act(command);
        
        // Assert
        _tokenStorage
            .Received(1)
            .Set(Arg.Is<TokensDto>(arg 
                    => arg.Token == token
                    && arg.RefreshToken == refreshToken));
    }

    [Fact]
    public async Task GivenNotExistingRefreshToken_WhenHandleAsync_ThenThrowsNotFoundExceptionWithCode_Refresh_RefreshToken()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var command = new RefreshCommand(AccountId.New(), Guid.NewGuid().ToString());

        _refreshTokenManager
            .DoesRefreshTokenExistAsync(command.RefreshToken, command.AccountId, cancellationToken)
            .Returns(false);
        
        // Act
        var exception = await Record.ExceptionAsync(() => Act(command));
        
        // Assert
        exception.ShouldBeOfType<NotFoundException>();
        ((NotFoundException)exception).Code.ShouldBe("Refresh.RefreshToken");
    }

    [Fact]
    public async Task GivenNotExistingAccount_WhenHandleAsync_ThenThrowsNotFoundExceptionWithCode_Refresh_Account()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var command = new RefreshCommand(AccountId.New(), Guid.NewGuid().ToString());
        
        _refreshTokenManager
            .DoesRefreshTokenExistAsync(command.RefreshToken, command.AccountId, cancellationToken)
            .Returns(true);

        _readAccountRepository
            .GetByIdAsync(command.AccountId, cancellationToken)
            .ReturnsNull();
        
        // Act
        var exception = await Record.ExceptionAsync(() => Act(command));
        
        // Assert
        exception.ShouldBeOfType<NotFoundException>();
        ((NotFoundException)exception).Code.ShouldBe("Refresh.Account");
    }
    
    [Fact]
    public async Task GivenNotExistingSubscription_WhenHandleAsync_ThenThrowsNotFoundExceptionWithCode_Refresh_Subscription()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var account = AccountFakeDataFactory
            .Get()
            .WithSubscriptionOrder();
        var command = new RefreshCommand(account.Id, Guid.NewGuid().ToString());
        
        _refreshTokenManager
            .DoesRefreshTokenExistAsync(command.RefreshToken, command.AccountId, cancellationToken)
            .Returns(true);

        _readAccountRepository
            .GetByIdAsync(command.AccountId, cancellationToken)
            .Returns(account);
        
        _readSubscriptionRepository
            .GetByIdAsync(account.ActiveSubscriptionOrder!.SubscriptionId, cancellationToken)
            .ReturnsNull();
        
        // Act
        var exception = await Record.ExceptionAsync(() => Act(command));
        
        // Assert
        exception.ShouldBeOfType<NotFoundException>();
        ((NotFoundException)exception).Code.ShouldBe("Refresh.Subscription");
    }
    
    [Fact]
    public async Task GivenNotExistingSubscription_WhenHandleAsync_ThenThrowsNotFoundExceptionWith_Refresh_Subscription()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var account = AccountFakeDataFactory
            .Get()
            .WithSubscriptionOrder();
        
        var command = new RefreshCommand(account.Id, Guid.NewGuid().ToString());
        
        _refreshTokenManager
            .DoesRefreshTokenExistAsync(command.RefreshToken, command.AccountId, cancellationToken)
            .Returns(true);
         
        _readAccountRepository
            .GetByIdAsync(command.AccountId, cancellationToken)
            .Returns(account);
         
        _readSubscriptionRepository
            .GetByIdAsync(account.ActiveSubscriptionOrder!.SubscriptionId, cancellationToken)
            .ReturnsNull();
         
        // Act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
         
        // Assert
        exception.ShouldBeOfType<NotFoundException>();
        ((NotFoundException)exception).Code.ShouldBe("Refresh.Subscription");
    }
    
    #region Arrange

    private readonly IRefreshTokenManager _refreshTokenManager;
    private readonly IReadAccountRepository _readAccountRepository;
    private readonly IReadSubscriptionRepository _readSubscriptionRepository;
    private readonly IAuthenticator _authenticator;
    private readonly ITokenStorage _tokenStorage;
    private readonly ICommandHandler<RefreshCommand> _handler;

    public RefreshCommandHandlerTests()
    {
        _readAccountRepository = Substitute.For<IReadAccountRepository>();
        _readSubscriptionRepository = Substitute.For<IReadSubscriptionRepository>();
        _refreshTokenManager = Substitute.For<IRefreshTokenManager>();
        _authenticator = Substitute.For<IAuthenticator>();
        _tokenStorage = Substitute.For<ITokenStorage>();
        _handler = new RefreshCommandHandler(
            _readAccountRepository,
            _readSubscriptionRepository,
            _refreshTokenManager,
            _authenticator,
            _tokenStorage);
    }

    #endregion
}