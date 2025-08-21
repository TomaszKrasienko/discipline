namespace discipline.centre.users.domain.Accounts.Services.Abstractions;

public interface IPasswordManager
{
    string Secure(string password);
    bool VerifyPassword(string securedPassword, string password);
}