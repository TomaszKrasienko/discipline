using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.users.application.Accounts.Commands;
using discipline.centre.users.application.Accounts.DTOs;
using discipline.centre.users.application.Accounts.Services;
using discipline.centre.users.domain.Accounts.Repositories;
using discipline.centre.users.domain.Accounts.Services.Abstractions;
using discipline.centre.users.domain.Subscriptions.Repositories;
using discipline.centre.users.tests.shared_kernel.Domain;
using discipline.libs.cqrs.abstractions.Commands;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Shouldly;
using Xunit;

namespace discipline.centre.users.application.unittests.Accounts.Commands;

public sealed class SignInCommandHandlerTests
{
     private Task Act(SignInCommand command) => _handler.HandleAsync(command, CancellationToken.None);

     [Fact]
     public async Task GivenExistingAccountWithValidPassword_WhenHandleAsync_ThenSavesTokensByTokenStorage()
     {
         // Arrange
         var cancellationToken = CancellationToken.None;
         var account = AccountFakeDataFactory
             .Get()
             .WithSubscriptionOrder(true);
         var command = new SignInCommand(account.Login.Value, "Test123!");
         
         _readWriteAccountRepository
             .GetByLoginAsync(command.Email, cancellationToken)
             .Returns(account);
         
         _passwordManager
             .VerifyPassword(account.Password.Value, command.Password)
             .Returns(true);

         var subscription = SubscriptionFakeDataFactory
             .GetPremium();
         
         _readSubscriptionRepository
             .GetByIdAsync(account.ActiveSubscriptionOrder!.SubscriptionId, cancellationToken)
             .Returns(subscription);

         var token = Guid.NewGuid().ToString();
         
         _authenticator
             .CreateToken(
                 account.Id,
                 subscription.Type.HasExpiryDate,
                 account.ActiveSubscriptionOrder!.Interval.FinishDate,
                 subscription.GetAllowedNumberOfDailyTasks(),
                 subscription.GetAllowedNumberOfRules())
             .Returns(token);

         var refreshToken = Guid.NewGuid().ToString();
         
         _refreshTokenFacade
             .GenerateAndReplaceAsync(account.Id, cancellationToken)
             .Returns(refreshToken);
         
         //act
         await Act(command);
         
         //assert
         _tokenStorage
             .Received(1)
             .Set(Arg.Is<TokensDto>(arg
                 => arg.Token == token
                 && arg.RefreshToken == refreshToken));
     }
     
     [Fact]
     public async Task GivenNotExistingEmail_WhenHandleAsync_ShouldThrowNotFoundExceptionWithCode_SignIn_Account()
     {
         // Arrange
         var command = new SignInCommand("test@test.pl", "Test123!");
         
         // Act
         var exception = await Record.ExceptionAsync( async() => await Act(command));
         
         // Assert
         exception.ShouldBeOfType<NotFoundException>();
         ((NotFoundException)exception).Code.ShouldBe("SignIn.Account");
     }

     [Fact]
     public async Task HandleAsync_GivenInvalidPassword_ShouldThrowInvalidPasswordExceptionWithCode_SignIn_Password()
     {
         // Arrange
         var cancellationToken = CancellationToken.None;
         var account = AccountFakeDataFactory
             .Get()
             .WithSubscriptionOrder();
         
         var command = new SignInCommand(account.Login.Value, "Test123!");
         
         _readWriteAccountRepository
             .GetByLoginAsync(command.Email, cancellationToken)
             .Returns(account);
         
         _passwordManager
             .VerifyPassword(account.Password.Value, command.Password)
             .Returns(false);
         
         // Act
         var exception = await Record.ExceptionAsync(async () => await Act(command));
         
         // Assert
         exception.ShouldBeOfType<InvalidArgumentException>();
         ((InvalidArgumentException)exception).Code.ShouldBe("SignIn.Password");
     }

     [Fact]
     public async Task GivenExistingAccountWithoutActiveSubscriptionOrder_WhenHandleAsync_ThenThrowsInvalidArgumentExceptionWith_SignIn_NullActiveSubscriptionOrder()
     {
         // Arrange
         var cancellationToken = CancellationToken.None;
         var account = AccountFakeDataFactory
             .Get();
         
         var command = new SignInCommand(account.Login.Value, "Test123!");
         
         _readWriteAccountRepository
             .GetByLoginAsync(command.Email, cancellationToken)
             .Returns(account);
         
         _passwordManager
             .VerifyPassword(account.Password.Value, command.Password)
             .Returns(true);
         
         // Act
         var exception = await Record.ExceptionAsync(async () => await Act(command));
         
         // Assert
         exception.ShouldBeOfType<InvalidArgumentException>();
         ((InvalidArgumentException)exception).Code.ShouldBe("SignIn.NullActiveSubscriptionOrder");
     }
     
     [Fact]
     public async Task GivenNotExistingSubscription_WhenHandleAsync_ThenThrowsNotFoundExceptionWith_SignIn_Subscription()
     {
         // Arrange
         var cancellationToken = CancellationToken.None;
         var account = AccountFakeDataFactory
             .Get()
             .WithSubscriptionOrder();
         
         var command = new SignInCommand(account.Login.Value, "Test123!");
         
         _readWriteAccountRepository
             .GetByLoginAsync(command.Email, cancellationToken)
             .Returns(account);
         
         _passwordManager
             .VerifyPassword(account.Password.Value, command.Password)
             .Returns(true);

         _readSubscriptionRepository
             .GetByIdAsync(account.ActiveSubscriptionOrder!.SubscriptionId, cancellationToken)
             .ReturnsNull();
         
         // Act
         var exception = await Record.ExceptionAsync(async () => await Act(command));
         
         // Assert
         exception.ShouldBeOfType<NotFoundException>();
         ((NotFoundException)exception).Code.ShouldBe("SignIn.Subscription");
     }
     
    #region Arrange

    private readonly IReadWriteAccountRepository _readWriteAccountRepository;
    private readonly IReadSubscriptionRepository _readSubscriptionRepository;
    private readonly IPasswordManager _passwordManager;
    private readonly IAuthenticator _authenticator;
    private readonly ITokenStorage _tokenStorage;
    private readonly IRefreshTokenManager _refreshTokenFacade;
    private readonly ICommandHandler<SignInCommand> _handler;

    public SignInCommandHandlerTests()
    {
        _readWriteAccountRepository = Substitute.For<IReadWriteAccountRepository>();
        _readSubscriptionRepository = Substitute.For<IReadSubscriptionRepository>();
        _passwordManager = Substitute.For<IPasswordManager>();
        _authenticator = Substitute.For<IAuthenticator>();
        _tokenStorage = Substitute.For<ITokenStorage>();
        _refreshTokenFacade = Substitute.For<IRefreshTokenManager>();
        _handler = new SignInCommandHandler(
            _readWriteAccountRepository,
            _readSubscriptionRepository,
            _passwordManager,
            _authenticator,
            _tokenStorage,
            _refreshTokenFacade);
    }

    #endregion
}