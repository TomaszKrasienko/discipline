using discipline.centre.users.application.Accounts.Commands;

// ReSharper disable once CheckNamespace
namespace discipline.centre.users.application.Accounts.DTOs.Requests;

public static class SignInRequestDtoMapperExtensions
{
    public static SignInCommand ToCommand(this SignInRequestDto request) 
        => new(request.Email, request.Password);
}