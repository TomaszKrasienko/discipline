using discipline.centre.users.domain.Accounts.Services;
using discipline.centre.users.domain.Accounts.Services.Abstractions;

namespace discipline.centre.users.integrationTests.Helpers;

internal sealed class TestsPasswordManager : IPasswordManager
{
    public string Secure(string password)
        => password;

    public bool VerifyPassword(string securedPassword, string password)
        => securedPassword == password;
}