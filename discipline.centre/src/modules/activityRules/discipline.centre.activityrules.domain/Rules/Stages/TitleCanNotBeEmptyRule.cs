using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.activityrules.domain.Rules.Stages;

internal sealed class TitleCanNotBeEmptyRule(string value) : IBusinessRule
{
    public Exception Exception => new DomainException("ActivityRule.Stage.Title.EmptyValue");

    public bool IsBroken()
        => string.IsNullOrWhiteSpace(value);
}