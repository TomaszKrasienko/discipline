using discipline.centre.users.domain.Accounts;
using discipline.centre.users.domain.Accounts.Services.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace discipline.centre.users.infrastructure.Passwords;

internal sealed class PasswordManager(IPasswordHasher<Account> passwordHasher) : IPasswordManager
{
    public string Secure(string password)
        => passwordHasher.HashPassword(null!, password);

    public bool VerifyPassword(string securedPassword, string password)
        => passwordHasher
            .VerifyHashedPassword(null!, securedPassword, password) == PasswordVerificationResult.Success;
}