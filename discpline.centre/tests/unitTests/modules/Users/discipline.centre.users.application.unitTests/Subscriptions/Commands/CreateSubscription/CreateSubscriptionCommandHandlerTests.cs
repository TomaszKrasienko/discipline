using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.application.Subscriptions.Commands.CreateSubscription;
using discipline.centre.users.domain.Subscriptions;
using discipline.centre.users.domain.Subscriptions.Enums;
using discipline.centre.users.domain.Subscriptions.Policies;
using discipline.centre.users.domain.Subscriptions.Repositories;
using discipline.centre.users.domain.Subscriptions.Specifications;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.centre.users.application.unittests.Subscriptions.Commands.CreateSubscription;

public sealed class CreateSubscriptionCommandHandlerTests
{
    private Task Act(CreateSubscriptionCommand command) => _handler.HandleAsync(command, CancellationToken.None);

    [Fact]
    public async Task GivenUniqueTitleAndValidParameters_WhenHandleAsync_ShouldAddSubscriptionByRepository()
    {
        // Arrange
        var command = new CreateSubscriptionCommand(
            SubscriptionId.New(),
            SubscriptionType.Premium,
            [new PriceSpecification(12, 123, Currency.Pln)]);

        _readWriteSubscriptionRepository
            .DoesTypeExistAsync(command.Type.Value, CancellationToken.None)
            .Returns(false);
        
        // Act
        await Act(command);
        
        // Assert
        await _readWriteSubscriptionRepository
            .Received(1)
            .AddAsync(Arg.Is<Subscription>(arg
                => arg.Id == command.Id 
                && arg.Type == command.Type
                && arg.Prices.Any(x 
                    => x.PerMonth == command.Prices.First().PerMonth
                    && x.PerYear == command.Prices.First().PerYear
                    && x.Currency == command.Prices.First().Currency)));
    }

    [Fact]
    public async Task GivenNotUniqueType_WhenHandleAsync_ShouldThrowNotUniqueExceptionWithCode_CreateSubscription_SubscriptionType()
    {
        // Arrange
        var command = new CreateSubscriptionCommand(
            SubscriptionId.New(),
            SubscriptionType.Premium,
            [new PriceSpecification(12, 123, Currency.Pln)]);

        _readWriteSubscriptionRepository
            .DoesTypeExistAsync(command.Type.Value, CancellationToken.None)
            .Returns(true);
        
        // Act
        var exception = await Record.ExceptionAsync(() => Act(command));
        
        // Assert
        exception.ShouldBeOfType<NotUniqueException>();
        ((NotUniqueException)exception).Code.ShouldBe("CreateSubscription.SubscriptionType");
    }
    
    #region Arrange

    private readonly IReadWriteSubscriptionRepository _readWriteSubscriptionRepository;
    private readonly ICommandHandler<CreateSubscriptionCommand> _handler;

    public CreateSubscriptionCommandHandlerTests()
    {
        _readWriteSubscriptionRepository = Substitute.For<IReadWriteSubscriptionRepository>();
        _handler = new CreateSubscriptionCommandHandler(
            _readWriteSubscriptionRepository,
            [
                new AdminSubscriptionPolicy(),
                new PremiumSubscriptionPolicy(),
                new StandardSubscriptionPolicy()
            ]);
    }
    #endregion
}