using discipline.daily_trackers.domain.DailyTrackers.Specifications;
using discipline.daily_trackers.domain.SharedKernel;
using discipline.daily_trackers.domain.SharedKernel.Exceptions;

namespace discipline.daily_trackers.domain.DailyTrackers.BusinessRules.Stages;

internal sealed class StageTitleMustBeUniqueRule(List<Stage>? stages, StageSpecification specification) : IBusinessRule
{
    public Exception Exception => new DomainException("ActivityRule.Stages.StageTitleMustBeUnique",
        "Stages must be unique.");
    public bool IsBroken()
        => stages is not null && stages.Exists(x => x.Title == specification.Title);
}