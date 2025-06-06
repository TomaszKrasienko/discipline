using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.activityrules.domain.Rules.Stages;

internal sealed class IndexCannotBeLessThan1Rule(int value) : IBusinessRule
{
    public Exception Exception => new DomainException("ActivityRule.Stage.Index.ValueBelowOne");

    public bool IsBroken()
        => value < 1;
}