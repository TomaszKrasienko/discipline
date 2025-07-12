using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.users.application.Users.DTOs;
using discipline.centre.users.application.Users.Exceptions;
using discipline.centre.users.application.Users.Services;
using discipline.centre.users.domain.Accounts.Services.Abstractions;
using discipline.centre.users.domain.Users.Repositories;

namespace discipline.centre.users.application.Accounts.Commands;

public sealed record SignInCommand(string Email, string Password) : ICommand;

// internal sealed class SignInCommandHandler(
//     IReadUserRepository readUserRepository,
//     IPasswordManager passwordManager,
//     IAuthenticator authenticator,
//     ITokenStorage tokenStorage,
//     IRefreshTokenManager refreshTokenFacade) : ICommandHandler<SignInCommand>
// {
//     public async Task HandleAsync(SignInCommand command, CancellationToken cancellationToken = default)
//     {
//         var user = await readUserRepository.GetByEmailAsync(command.Email, cancellationToken);
//         if (user is null)
//         {
//             throw new SignInException();
//         }
//
//         var isPasswordValid = passwordManager.VerifyPassword(user.Password.HashedValue!, command.Password);
//         if (!isPasswordValid)
//         {
//             throw new SignInException();
//         }
//
//         var token = authenticator.CreateToken(user.Id.ToString(), user.Email, user.Status);
//         var refreshToken = await refreshTokenFacade.GenerateAndSaveAsync(user.Id, cancellationToken);
//         tokenStorage.Set(new TokensDto(token, refreshToken));
//     }
// }