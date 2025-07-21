using discipline.centre.users.domain.Accounts.Services.Abstractions;
using discipline.centre.users.domain.Users;
using Microsoft.AspNetCore.Identity;

namespace discipline.centre.users.infrastructure.Accounts.Passwords;

internal sealed class PasswordManager(
    IPasswordHasher<User> passwordHasher) : IPasswordManager
{
    public string Secure(string password)
        => passwordHasher.HashPassword(null!, password);

    public bool VerifyPassword(string securedPassword, string password)
        => passwordHasher
            .VerifyHashedPassword(null!, securedPassword, password) == PasswordVerificationResult.Success;
}