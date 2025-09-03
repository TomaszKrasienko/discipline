using discipline.hangfire.shared.abstractions.ViewModels;

namespace discipline.hangfire.shared.abstractions.Api;

public interface IAccountModificationApi
{
    Task<IReadOnlyCollection<AccountViewModel>> GetAccountWithoutActivityRules(CancellationToken cancellationToken = default);
}