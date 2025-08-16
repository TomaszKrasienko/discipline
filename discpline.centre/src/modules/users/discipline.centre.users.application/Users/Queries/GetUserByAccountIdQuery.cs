using discipline.centre.shared.abstractions.CQRS.Queries;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.application.Users.DTOs.Responses;

namespace discipline.centre.users.application.Users.Queries;

public sealed record GetUserByAccountIdQuery(AccountId AccountId) : IQuery<UserResponseDto?>;