using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.application.Accounts.Commands;

// ReSharper disable once CheckNamespace
namespace discipline.centre.users.application.Accounts.DTOs.Requests;

public static class RefreshRequestDtoMapperExtensions
{
    //TODO: unit tests
    public static RefreshCommand ToCommand(this RefreshRequestDto dto)
        => new(
            AccountId.Parse(dto.AccountId),
            dto.RefreshToken);
}