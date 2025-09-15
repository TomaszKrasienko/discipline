using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.application.Accounts.DTOs;
using discipline.centre.users.application.Accounts.Services;
using discipline.centre.users.domain.Accounts.Repositories;
using discipline.centre.users.domain.Subscriptions.Repositories;
using discipline.libs.cqrs.abstractions.Commands;

namespace discipline.centre.users.application.Accounts.Commands;

public sealed record RefreshCommand(
    AccountId AccountId,
    string RefreshToken) : ICommand;

internal sealed class RefreshCommandHandler(
    IReadAccountRepository readAccountRepository,
    IReadSubscriptionRepository readSubscriptionRepository,
    IRefreshTokenManager refreshTokenManager,
    IAuthenticator authenticator,
    ITokenStorage tokenStorage) : ICommandHandler<RefreshCommand>
{
    public async Task HandleAsync(RefreshCommand command, CancellationToken cancellationToken)
    {
        var doesRefreshTokenExist = await refreshTokenManager.DoesRefreshTokenExistAsync(
            command.RefreshToken,
            command.AccountId, 
            cancellationToken);

        if (!doesRefreshTokenExist)
        {
            throw new NotFoundException("Refresh.RefreshToken");
        }
        
        var account = await readAccountRepository.GetByIdAsync(command.AccountId, cancellationToken);

        if (account is null)
        {
            throw new NotFoundException("Refresh.Account");
        }
        
        var subscription = await readSubscriptionRepository.GetByIdAsync(account!.ActiveSubscriptionOrder!.SubscriptionId, cancellationToken);

        if (subscription is null)
        {
            throw new NotFoundException("Refresh.Subscription");
        }
        
        var token = authenticator.CreateToken(
                account.Id, 
                subscription.Type.HasExpiryDate,
                account.ActiveSubscriptionOrder.Interval.FinishDate,
                subscription.GetAllowedNumberOfDailyTasks(),
                subscription.GetAllowedNumberOfRules());
        var refreshToken = await refreshTokenManager.GenerateAndReplaceAsync(account.Id, cancellationToken);

        tokenStorage.Set(new TokensDto(token, refreshToken));
    }
}