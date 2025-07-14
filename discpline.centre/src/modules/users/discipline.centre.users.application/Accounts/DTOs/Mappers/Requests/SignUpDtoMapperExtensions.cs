using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.application.Accounts.Commands;
using discipline.centre.users.domain.Subscriptions.Enums;

// ReSharper disable once CheckNamespace
namespace discipline.centre.users.application.Accounts.DTOs.Requests;

public static class SignUpDtoMapperExtensions
{
    //TODO: Unit tests
    public static SignUpCommand MapAsCommand(
        this SignUpRequestDto request,
        AccountId accountId)
        => new(
            accountId,
            request.Email,
            request.Password,
            SubscriptionId.Parse(request.SubscriptionId),
            Period.FromValue(request.Period),
            request.FirstName,
            request.LastName,
            request.PaymentValue);  
    
}