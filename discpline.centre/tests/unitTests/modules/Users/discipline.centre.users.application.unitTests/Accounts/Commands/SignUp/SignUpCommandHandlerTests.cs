using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.shared.abstractions.UnitOfWork;
using discipline.centre.users.application.Accounts.Commands;
using discipline.centre.users.domain.Accounts;
using discipline.centre.users.domain.Accounts.Repositories;
using discipline.centre.users.domain.Accounts.Services.Abstractions;
using discipline.centre.users.domain.Accounts.Specifications.Account;
using discipline.centre.users.domain.Accounts.Specifications.SubscriptionOrder;
using discipline.centre.users.domain.Subscriptions.Enums;
using discipline.centre.users.domain.Subscriptions.Repositories;
using discipline.centre.users.domain.Users;
using discipline.centre.users.domain.Users.Repositories;
using discipline.centre.users.tests.sharedkernel.Domain;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using Shouldly;
using Xunit;

namespace discipline.centre.users.application.unittests.Accounts.Commands.SignUp;

public sealed class SignUpCommandHandlerTests
{
    private Task Act(SignUpCommand command) => _handler.HandleAsync(command, CancellationToken.None);

    [Fact]
    public async Task GivenNotRegisteredEmailAndExistingSubscription_WhenHandleAsync_ThenCallsStartTransaction()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var subscription = SubscriptionFakeDataFactory.GetStandard();
        var now = DateTime.UtcNow;
        
        _timeProvider
            .GetUtcNow()
            .Returns(now);
        
        var command = new SignUpCommand(
            AccountId.New(),
            "test@test.pl",
            "Test123!",
            SubscriptionId.New(), 
            Period.Month,
            "test_first_name",
            "test_last_name",
            null);

        var account = Account.Create(
            command.AccountId,
            command.Email,
            new PasswordSpecification(command.Password, Guid.NewGuid().ToString()),
            _timeProvider,
            new SubscriptionOrderSpecification(
                subscription.Id,
                subscription.Type.Value,
                command.Period,
                subscription.Type.HasPayment,
                command.PaymentValue));

        _readWriteAccountRepository
            .DoesEmailExistAsync(command.Email, cancellationToken)
            .Returns(false);
        
        _readSubscriptionRepository
            .GetByIdAsync(command.SubscriptionId, cancellationToken)
            .Returns(subscription);

        _accountService
            .Create(
                command.AccountId,
                command.Email,
                command.Password,
                new SubscriptionOrderSpecification(
                    subscription.Id,
                    subscription.Type.Value,
                    command.Period,
                    subscription.Type.HasPayment,
                    command.PaymentValue))
            .Returns(account);
        
        // Act
        await Act(command);
        
        // Assert
        await _unitOfWork
            .Received(1)
            .StartTransactionAsync(cancellationToken);
    }
    
    [Fact]
    public async Task GivenNotRegisteredEmailndExistingSubscription_WhenHandleAsync_ThenAddsAccountAndUser()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var subscription = SubscriptionFakeDataFactory.GetStandard();
        var now = DateTime.UtcNow;
        
        _timeProvider
            .GetUtcNow()
            .Returns(now);
        
        var command = new SignUpCommand(
            AccountId.New(),
            "test@test.pl",
            "Test123!",
            SubscriptionId.New(), 
            Period.Month,
            "test_first_name",
            "test_last_name",
            null);

        var account = Account.Create(
            command.AccountId,
            command.Email,
            new PasswordSpecification(command.Password, Guid.NewGuid().ToString()),
            _timeProvider,
            new SubscriptionOrderSpecification(
                subscription.Id,
                subscription.Type.Value,
                command.Period,
                subscription.Type.HasPayment,
                command.PaymentValue));

        _readWriteAccountRepository
            .DoesEmailExistAsync(command.Email, cancellationToken)
            .Returns(false);
        
        _readSubscriptionRepository
            .GetByIdAsync(command.SubscriptionId, cancellationToken)
            .Returns(subscription);

        _accountService
            .Create(
                command.AccountId,
                command.Email,
                command.Password,
                new SubscriptionOrderSpecification(
                    subscription.Id,
                    subscription.Type.Value,
                    command.Period,
                    subscription.Type.HasPayment,
                    command.PaymentValue))
            .Returns(account);
        
        // Act
        await Act(command);
        
        // Assert
        await _readWriteAccountRepository
            .Received(1)
            .AddAsync(account, cancellationToken);
        
        await _readWriteUserRepository
            .Received(1)
            .AddAsync(Arg.Is<User>(
                arg 
                    => arg.Email.Value == command.Email
                    && arg.FullName.FirstName == command.FirstName
                    && arg.FullName.LastName == command.LastName
                    && arg.AccountId == command.AccountId), cancellationToken);
    }

    [Fact]
    public async Task GivenSuccessfullyAddedAccountAndUser_WhenHandleAsync_ThenCallsCommitTransactionAsync()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var subscription = SubscriptionFakeDataFactory.GetStandard();
        var now = DateTime.UtcNow;
        
        _timeProvider
            .GetUtcNow()
            .Returns(now);
        
        var command = new SignUpCommand(
            AccountId.New(),
            "test@test.pl",
            "Test123!",
            SubscriptionId.New(), 
            Period.Month,
            "test_first_name",
            "test_last_name",
            null);

        var account = Account.Create(
            command.AccountId,
            command.Email,
            new PasswordSpecification(command.Password, Guid.NewGuid().ToString()),
            _timeProvider,
            new SubscriptionOrderSpecification(
                subscription.Id,
                subscription.Type.Value,
                command.Period,
                subscription.Type.HasPayment,
                command.PaymentValue));

        _readWriteAccountRepository
            .DoesEmailExistAsync(command.Email, cancellationToken)
            .Returns(false);
        
        _readSubscriptionRepository
            .GetByIdAsync(command.SubscriptionId, cancellationToken)
            .Returns(subscription);

        _accountService
            .Create(
                command.AccountId,
                command.Email,
                command.Password,
                new SubscriptionOrderSpecification(
                    subscription.Id,
                    subscription.Type.Value,
                    command.Period,
                    subscription.Type.HasPayment,
                    command.PaymentValue))
            .Returns(account);
        
        // Act
        await Act(command);
        
        // Assert
        await _unitOfWork
            .Received(1)
            .CommitTransactionAsync(cancellationToken)!;
    }
    
    [Fact]
    public async Task GivenExistingEmail_WhenHandleAsync_ThenThrowsNotUniqueExceptionWithCode_SignUpCommand_Email()
    {
        // Arrange
        var command = new SignUpCommand(
            AccountId.New(),
            "test@test.pl",
            "Test123!",
            SubscriptionId.New(), 
            Period.Month,
            "test_first_name",
            "test_last_name",
            null);

        _readWriteAccountRepository
            .DoesEmailExistAsync(command.Email, CancellationToken.None)
            .Returns(true);
        
        // Act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        // Assert
        exception.ShouldBeOfType<NotUniqueException>();
        ((NotUniqueException)exception).Code.ShouldBe("SignUpCommand.Email");
    }
    
    [Fact]
    public async Task GivenNotExistingSubscription_WhenHandleAsync_ThenThrowsNotFoundExceptionWithCode_SignUpCommand_Subscription()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        
        var command = new SignUpCommand(
            AccountId.New(),
            "test@test.pl",
            "Test123!",
            SubscriptionId.New(), 
            Period.Month,
            "test_first_name",
            "test_last_name",
            null);

        _readWriteAccountRepository
            .DoesEmailExistAsync(command.Email, cancellationToken)
            .Returns(false);

        _readSubscriptionRepository
            .GetByIdAsync(command.SubscriptionId, cancellationToken)
            .ReturnsNull();
        
        // Act
        var exception = await Record.ExceptionAsync(() => Act(command));
        
        // Assert
        exception.ShouldBeOfType<NotFoundException>();
        ((NotFoundException)exception).Code.ShouldBe("SignUpCommand.Subscription");
    }

    [Fact]
    public async Task GivenExceptionFromAddAccountAsync_WhenHandleAsync_ThenCallsRollbackTransactionAsync()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var subscription = SubscriptionFakeDataFactory.GetStandard();
        var now = DateTime.UtcNow;
        
        _timeProvider
            .GetUtcNow()
            .Returns(now);
        
        var command = new SignUpCommand(
            AccountId.New(),
            "test@test.pl",
            "Test123!",
            SubscriptionId.New(), 
            Period.Month,
            "test_first_name",
            "test_last_name",
            null);

        var account = Account.Create(
            command.AccountId,
            command.Email,
            new PasswordSpecification(command.Password, Guid.NewGuid().ToString()),
            _timeProvider,
            new SubscriptionOrderSpecification(
                subscription.Id,
                subscription.Type.Value,
                command.Period,
                subscription.Type.HasPayment,
                command.PaymentValue));

        _readWriteAccountRepository
            .DoesEmailExistAsync(command.Email, cancellationToken)
            .Returns(false);
        
        _readSubscriptionRepository
            .GetByIdAsync(command.SubscriptionId, cancellationToken)
            .Returns(subscription);

        _accountService
            .Create(
                command.AccountId,
                command.Email,
                command.Password,
                new SubscriptionOrderSpecification(
                    subscription.Id,
                    subscription.Type.Value,
                    command.Period,
                    subscription.Type.HasPayment,
                    command.PaymentValue))
            .Returns(account);

        _readWriteAccountRepository
            .AddAsync(account, cancellationToken)
            .ThrowsAsync(new Exception());
        
        // Act
        _ = await Record.ExceptionAsync(() => Act(command));
        
        // Assert
        await _unitOfWork
            .Received(1)
            .RollbackTransactionAsync(cancellationToken)!;
    }

    #region Arrange

    private readonly IReadWriteAccountRepository _readWriteAccountRepository;
    private readonly IReadWriteUserRepository _readWriteUserRepository;
    private readonly IReadSubscriptionRepository _readSubscriptionRepository;
    private readonly IAccountService _accountService;
    private IUnitOfWork _unitOfWork;
    private readonly TimeProvider _timeProvider;
    private readonly ICommandHandler<SignUpCommand> _handler;

    public SignUpCommandHandlerTests()
    {
        _readWriteAccountRepository = Substitute.For<IReadWriteAccountRepository>();
        _readWriteUserRepository = Substitute.For<IReadWriteUserRepository>();
        _readSubscriptionRepository = Substitute.For<IReadSubscriptionRepository>();
        _accountService = Substitute.For<IAccountService>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _timeProvider = Substitute.For<TimeProvider>();
        _handler = new SignUpCommandHandler(
            _readWriteAccountRepository,
            _readWriteUserRepository,
            _readSubscriptionRepository,
            _accountService,
            _unitOfWork);
    }
    #endregion
}