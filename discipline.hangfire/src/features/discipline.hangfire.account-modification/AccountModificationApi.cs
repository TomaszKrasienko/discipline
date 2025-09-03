using discipline.hangfire.shared.abstractions.Api;
using discipline.hangfire.shared.abstractions.ViewModels;

namespace discipline.hangfire.account_modification;

internal sealed class AccountModificationApi : IAccountModificationApi
{
    public Task<IReadOnlyCollection<AccountViewModel>> GetAccountWithoutActivityRules(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}