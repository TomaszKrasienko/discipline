using discipline.daily_trackers.domain.DailyTrackers.Specifications;
using discipline.daily_trackers.domain.SharedKernel;
using discipline.daily_trackers.domain.SharedKernel.Exceptions;

namespace discipline.daily_trackers.domain.DailyTrackers.BusinessRules.Stages;

internal sealed class StagesMustHaveOrderedIndexRule(List<Stage>? stages, StageSpecification specification) : IBusinessRule
{
    public Exception Exception => new DomainException("ActivityRule.Stages.MustHaveOrderedIndex",
        "Provided stages have invalid indexes");

    public bool IsBroken()
    {
        if (stages is null || stages.Count < 1)
        {
            return specification.Index != 1;
        }
        return (stages!.Max(x => x.Index)! + 1) != specification.Index;
    }
}