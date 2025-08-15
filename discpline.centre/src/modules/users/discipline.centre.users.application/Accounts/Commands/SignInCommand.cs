using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.users.application.Accounts.Services;
using discipline.centre.users.application.Users.DTOs;
using discipline.centre.users.domain.Accounts.Repositories;
using discipline.centre.users.domain.Accounts.Services.Abstractions;
using discipline.centre.users.domain.Subscriptions.Repositories;

namespace discipline.centre.users.application.Accounts.Commands;

public sealed record SignInCommand(string Email, string Password) : ICommand;

internal sealed class SignInCommandHandler(
    IReadAccountRepository readAccountRepository,
    IReadSubscriptionRepository readSubscriptionRepository,
    IPasswordManager passwordManager,
    IAuthenticator authenticator,
    ITokenStorage tokenStorage,
    IRefreshTokenManager refreshTokenManager) : ICommandHandler<SignInCommand>
{
    public async Task HandleAsync(SignInCommand command, CancellationToken cancellationToken = default)
    {
        var account = await readAccountRepository
            .GetByLoginAsync(command.Email, cancellationToken);

        if (account is null)
        {
            throw new NotFoundException("SignIn.Account");
        }

        var isPasswordValid = passwordManager.VerifyPassword(account.Password.Value, command.Password);
        
        if (!isPasswordValid)
        {
            throw new InvalidArgumentException("SignIn.Password");
        }

        var activeSubscriptionOrder = account.ActiveSubscriptionOrder;
        
        if (activeSubscriptionOrder is null)
        {
            throw new InvalidArgumentException("SignIn.NullActiveSubscriptionOrder");
        }
        
        var subscription = await readSubscriptionRepository
            .GetByIdAsync(
                activeSubscriptionOrder.SubscriptionId,
                cancellationToken);

        var token = authenticator.CreateToken(
            account.Id,
            subscription?.GetAllowedNumberOfDailyTasks(),
            subscription?.GetAllowedNumberOfRules());
        
        var refreshToken = await refreshTokenManager.GenerateAndReplaceAsync(account.Id, cancellationToken);
        tokenStorage.Set(new TokensDto(token, refreshToken));
    }
}