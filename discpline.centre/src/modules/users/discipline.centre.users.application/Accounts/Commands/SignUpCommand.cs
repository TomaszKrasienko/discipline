using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Events;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Accounts.Repositories;
using discipline.centre.users.domain.Accounts.Services.Abstractions;
using discipline.centre.users.domain.Accounts.Specifications.SubscriptionOrder;
using discipline.centre.users.domain.Subscriptions.Enums;
using discipline.centre.users.domain.Subscriptions.Repositories;

namespace discipline.centre.users.application.Accounts.Commands;

public sealed record SignUpCommand(
    AccountId AccountId,
    string Email, 
    string Password,
    SubscriptionId SubscriptionId,
    Period Period,
    string FirstName,
    string LastName) : ICommand;


internal sealed class SignUpCommandHandler(
    IReadWriteAccountRepository accountRepository,
    IReadSubscriptionRepository subscriptionRepository,
    IAccountService accountService,
    IEventProcessor eventProcessor) : ICommandHandler<SignUpCommand>
{
    public async Task HandleAsync(SignUpCommand command, CancellationToken cancellationToken = default)
    {
        var doesEmailExist = await accountRepository.DoesEmailExistAsync(
            command.Email,
            cancellationToken);
        
        if (doesEmailExist)
        {
            throw new NotUniqueException("SignUpCommand.Email", command.Email);
        }

        var subscription = await subscriptionRepository
            .GetByIdAsync(command.SubscriptionId, cancellationToken);

        if (subscription is null)
        {
            throw new NotFoundException("SignUpCommand.Subscription", command.SubscriptionId);
        }
        
        var subscriptionOrderSpecification = new SubscriptionOrderSpecification(
            subscription.Type.Value,
            subscription.)
        
        var account = accountService.Create(
            command.AccountId,
            command.Email,
            command.Password,
            )
        
        
        await eventProcessor.PublishAsync(user.DomainEvents.Select(x 
            => x.MapAsIntegrationEvent()).ToArray());
    }
}