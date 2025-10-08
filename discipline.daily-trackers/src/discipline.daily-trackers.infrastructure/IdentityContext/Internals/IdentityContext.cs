using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;
using discipline.daily_trackers.infrastructure.IdentityContext.Abstractions;
using Microsoft.AspNetCore.Http;

namespace discipline.daily_trackers.infrastructure.IdentityContext.Internals;

internal sealed class IdentityContext : IIdentityContext
{
    private readonly AccountId? _accountId;
    public bool IsAuthenticated { get; }
    public string? Status { get; }
    public AccountId? GetAccount()
        =>  _accountId ?? null;

    public IdentityContext(IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor.HttpContext is null)
        {
            return;
        }
        
        var account = httpContextAccessor.HttpContext.User;
        IsAuthenticated = account.Identity?.IsAuthenticated ?? false;

        if (!IsAuthenticated || account.Identity?.Name is null)
        {
            return;
        }

        _accountId = AccountId.Parse(account.Identity?.Name!);
    }
}