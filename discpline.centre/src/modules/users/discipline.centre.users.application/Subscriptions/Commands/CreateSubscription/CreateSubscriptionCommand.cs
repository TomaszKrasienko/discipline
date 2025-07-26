using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Subscriptions;
using discipline.centre.users.domain.Subscriptions.Enums;
using discipline.centre.users.domain.Subscriptions.Policies.Abstractions;
using discipline.centre.users.domain.Subscriptions.Repositories;
using discipline.centre.users.domain.Subscriptions.Specifications;
using FluentValidation;

namespace discipline.centre.users.application.Subscriptions.Commands.CreateSubscription;

public sealed record CreateSubscriptionCommand(
    SubscriptionId Id,
    SubscriptionType Type, 
    HashSet<PriceSpecification> Prices) : ICommand;

internal sealed class CreateSubscriptionCommandHandler(
    IReadWriteSubscriptionRepository readWriteSubscriptionRepository,
    IEnumerable<ISubscriptionPolicy> subscriptionPolicies) : ICommandHandler<CreateSubscriptionCommand>
{
    public async Task HandleAsync(CreateSubscriptionCommand command, CancellationToken cancellationToken = default)
    {
        var doesEmailExist = await readWriteSubscriptionRepository.DoesTypeExistAsync(
            command.Type.Value,
            cancellationToken);
        
        if (doesEmailExist)
        {
            throw new NotUniqueException("CreateSubscription.SubscriptionType", command.Type.Value);
        }

        var subscription = Subscription.Create(
            command.Id,
            command.Type,
            command.Prices,
            subscriptionPolicies);
        
        await readWriteSubscriptionRepository.AddAsync(subscription, cancellationToken);
    }
}