using discipline.centre.users.application.Accounts.DTOs;
using discipline.centre.users.application.Users.DTOs;

namespace discipline.centre.users.application.Accounts.Services;

public interface ITokenStorage
{
    void Set(TokensDto jwtDto);
    TokensDto? Get();
}