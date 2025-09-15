using discipline.centre.activityrules.application.ActivityRules.DTOs.Commands;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.application.ActivityRules.Commands.External;

public sealed record CreateActivityFromActivityRuleCommand(
    AccountId AccountId,
    DateOnly Day,
    CreateActivityDetailsDto Details);