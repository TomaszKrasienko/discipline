using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;

namespace discipline.daily_trackers.infrastructure.IdentityContext.Abstractions;

public interface IIdentityContext
{
    public bool IsAuthenticated { get; }
    public AccountId? GetAccount();
}