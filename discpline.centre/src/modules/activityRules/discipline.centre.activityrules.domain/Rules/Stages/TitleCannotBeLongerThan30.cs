using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.activityrules.domain.Rules.Stages;

internal sealed class TitleCannotBeLongerThan30(string value) : IBusinessRule
{
    public Exception Exception => new DomainException("ActivityRule.Stage.Title.ValueTooLong");

    public bool IsBroken()
        => value.Length > 30;
}