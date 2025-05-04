using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.activityrules.domain.Rules.ActivityRules;

internal sealed class DetailsTitleCannotBeEmptyRule(string value) : IBusinessRule
{
    public Exception Exception => new DomainException("ActivityRule.Details.EmptyTitle");

    public bool IsBroken()
        => string.IsNullOrWhiteSpace(value);
}