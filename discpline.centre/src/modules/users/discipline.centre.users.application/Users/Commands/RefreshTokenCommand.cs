using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.application.Users.DTOs;
using discipline.centre.users.application.Users.Exceptions;
using discipline.centre.users.application.Users.Services;
using discipline.centre.users.domain.Users.Repositories;

namespace discipline.centre.users.application.Users.Commands;

public sealed record RefreshTokenCommand(
    string RefreshToken,
    UserId UserId) : ICommand;

internal sealed class RefreshTokenCommandHandler(
    IRefreshTokenManager refreshTokenManager,
    IReadUserRepository readUserRepository,
    IAuthenticator authenticator,
    ITokenStorage tokenStorage) : ICommandHandler<RefreshTokenCommand>
{
    public async Task HandleAsync(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var doesRefreshTokenValid = await refreshTokenManager.DoesRefreshTokenExistsAsync(
            command.RefreshToken,
            command.UserId,
            cancellationToken);

        if (!doesRefreshTokenValid)
        {
            throw new SignInException();
        }
        
        var user = await readUserRepository.GetByIdAsync(command.UserId, cancellationToken);

        if (user is null)
        {
            throw new SignInException();
        }

        var token = authenticator.CreateToken(user.Id.ToString(), user.Email.Value, user.Status.Value);
        var refreshToken = await refreshTokenManager.GenerateAndSaveAsync(user.Id, cancellationToken);
        
        tokenStorage.Set(new TokensDto(token, refreshToken));
    }
}