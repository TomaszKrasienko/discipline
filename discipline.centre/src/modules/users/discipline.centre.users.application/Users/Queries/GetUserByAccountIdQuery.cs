using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.application.Users.DTOs.Responses;
using discipline.libs.cqrs.abstractions.Queries;

namespace discipline.centre.users.application.Users.Queries;

public sealed record GetUserByAccountIdQuery(AccountId AccountId) : IQuery<UserResponseDto?>;