using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Subscriptions;
using discipline.centre.users.domain.Subscriptions.Repositories;
using discipline.centre.users.domain.Subscriptions.Specifications;
using FluentValidation;

namespace discipline.centre.users.application.Subscriptions.Commands.CreateSubscription;

public sealed record CreateSubscriptionCommand(
    SubscriptionId Id,
    string Type, 
    HashSet<PriceSpecification> Prices) : ICommand;

internal sealed class CreateSubscriptionCommandHandler(
    IReadWriteSubscriptionRepository readWriteSubscriptionRepository) : ICommandHandler<CreateSubscriptionCommand>
{
    public async Task HandleAsync(CreateSubscriptionCommand command, CancellationToken cancellationToken = default)
    {
        var doesEmailExist = await readWriteSubscriptionRepository.DoesTitleExistAsync(command.Title, cancellationToken);
        
        if (doesEmailExist)
        {
            return;
        }

        var subscription = Subscription.Create(command.Id, command.Title, command.PricePerMonth,
            command.PricePerYear, command.Features);
        await readWriteSubscriptionRepository.AddAsync(subscription, cancellationToken);
    }
}