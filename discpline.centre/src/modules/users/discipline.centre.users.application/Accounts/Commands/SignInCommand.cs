using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.users.application.Users.DTOs;
using discipline.centre.users.application.Users.Exceptions;
using discipline.centre.users.application.Users.Services;
using discipline.centre.users.domain.Accounts.Repositories;
using discipline.centre.users.domain.Accounts.Services.Abstractions;
using discipline.centre.users.domain.Users.Repositories;

namespace discipline.centre.users.application.Accounts.Commands;

public sealed record SignInCommand(string Email, string Password) : ICommand;

internal sealed class SignInCommandHandler(
    IReadWriteAccountRepository readWriteAccountRepository,
    IReadUserRepository readUserRepository,
    IPasswordManager passwordManager,
    IAuthenticator authenticator,
    ITokenStorage tokenStorage,
    IRefreshTokenManager refreshTokenFacade) : ICommandHandler<SignInCommand>
{
    public async Task HandleAsync(SignInCommand command, CancellationToken cancellationToken = default)
    {
        var account = await readWriteAccountRepository
            .GetByEmailAsync(command.Email, cancellationToken);

        if (account is null)
        {
            throw new NotFoundException("SignIn.Account");
        }

        var isPasswordValid = passwordManager.VerifyPassword(account.Password.Value, command.Password);
        
        if (!isPasswordValid)
        {
            throw new InvalidArgumentException("SignIn.Password");
        }

        var token = authenticator.CreateToken(user.Id.ToString(), user.Email, user.Status);
        
        
        var refreshToken = await refreshTokenFacade.GenerateAndSaveAsync(user.Id, cancellationToken);
        tokenStorage.Set(new TokensDto(token, refreshToken));
    }
}