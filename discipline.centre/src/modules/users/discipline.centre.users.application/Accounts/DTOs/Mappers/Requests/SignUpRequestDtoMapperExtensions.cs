using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.application.Accounts.Commands;
using discipline.centre.users.domain.Subscriptions.Enums;

// ReSharper disable once CheckNamespace
namespace discipline.centre.users.application.Accounts.DTOs.Requests;

public static class SignUpRequestDtoMapperExtensions
{
    //TODO: Unit test
    public static SignUpCommand ToCommand(
        this SignUpRequestDto request,
        AccountId accountId)
        => new(
            accountId,
            request.Email,
            request.Password,
            SubscriptionId.Parse(request.SubscriptionId),
            request.Period is null 
                ? null 
                : Period.FromValue(request.Period),
            request.FirstName,
            request.LastName,
            request.PaymentValue);  
    
}