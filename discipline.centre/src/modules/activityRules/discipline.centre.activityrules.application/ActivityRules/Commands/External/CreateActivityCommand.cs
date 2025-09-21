using discipline.centre.activityrules.application.ActivityRules.DTOs.Commands;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.libs.cqrs.abstractions.Commands;

namespace discipline.centre.activityrules.application.ActivityRules.Commands.External;

public sealed record CreateActivityCommand(
    string AccountId,
    DateOnly Day,
    IReadOnlyCollection<CreateActivityDto> Activities) : ICommand;