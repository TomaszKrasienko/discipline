using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.users.application.Accounts.Commands;
using discipline.centre.users.application.Users.DTOs;
using discipline.centre.users.application.Users.Services;
using discipline.centre.users.domain.Accounts.Repositories;
using discipline.centre.users.domain.Accounts.Services.Abstractions;
using discipline.centre.users.domain.Subscriptions.Repositories;
using discipline.centre.users.tests.sharedkernel.Domain;
using NSubstitute;
using Xunit;

namespace discipline.centre.users.application.unittests.Accounts.SignIn;

public sealed class SignInCommandHandlerTests
{
     private Task Act(SignInCommand command) => _handler.HandleAsync(command, default);

     [Fact]
     public async Task GivenExistingAccountWithValidPassword_WhenHandleAsync_ThenSavesTokensByTokenStorage()
     {
         //arrange
         var cancellationToken = CancellationToken.None;
         var account = AccountFakeDataFactory
             .Get()
             .WithSubscriptionOrder(true);
         var command = new SignInCommand(account.Login.Value, "Test123!");
         
         _readWriteAccountRepository
             .GetByEmailAsync(command.Email, cancellationToken)
             .Returns(account);
         
         _passwordManager
             .VerifyPassword(account.Password.Value, command.Password)
             .Returns(true);

         var subscription = SubscriptionFakeDataFactory
             .GetPremium();

         var token = Guid.NewGuid().ToString();
         
         _authenticator
             .CreateToken(
                 account.Id,
                 subscription.GetAllowedNumberOfDailyTasks(),
                 subscription.GetAllowedNumberOfRules())
             .Returns(token);

         var refreshToken = Guid.NewGuid().ToString();
         
         _refreshTokenFacade
             .GenerateAndSaveAsync(account.Id, cancellationToken)
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
     
//     [Fact]
//     public async Task HandleAsync_GivenNotExistingEmail_ShouldThrowNotFoundException()
//     {
//         //arrange
//         var command = new SignInCommand("test@test.pl", "Test123!");
//         
//         //act
//         var exception = await Record.ExceptionAsync( async() => await Act(command));
//         
//         //assert
//         exception.ShouldBeOfType<NotFoundException>();
//     }
//
//     [Fact]
//     public async Task HandleAsync_GivenInvalidPassword_ShouldThrowInvalidPasswordException()
//     {
//         //arrange
//         var user = UserFakeDataFactory.Get();
//         var command = new SignInCommand(user.Email, "Test123!");
//         _readUserRepository
//             .GetByEmailAsync(command.Email, default)
//             .Returns(user);
//         
//         _passwordManager
//             .VerifyPassword(user.Password.HashedValue!, command.Password)
//             .Returns(false);
//         
//         //act
//         var exception = await Record.ExceptionAsync(async () => await Act(command));
//         
//         //assert
//         exception.ShouldBeOfType<InvalidPasswordException>();
//     }
    
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