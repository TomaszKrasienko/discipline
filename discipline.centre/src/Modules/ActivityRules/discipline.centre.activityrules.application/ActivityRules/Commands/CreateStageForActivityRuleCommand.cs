using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.application.ActivityRules.Commands;

public sealed record CreateStageForActivityRuleCommand(UserId UserId,
    ActivityRuleId ActivityRuleId,
    StageId StageId,
    );