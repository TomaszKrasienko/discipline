using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.activityrules.domain.Rules.ActivityRules;

internal sealed class DetailsTitleCannotBeLongerThan30Rule(string value) : IBusinessRule
{
    public Exception Exception => new DomainException("ActivityRule.Details.TitleTooLong");
    public bool IsBroken()
        => value.Length > 30;
}