namespace discipline.centre.users.domain.Accounts.Specifications.Account;

public sealed record PasswordSpecification(
    string Password, 
    string HashedPassword);