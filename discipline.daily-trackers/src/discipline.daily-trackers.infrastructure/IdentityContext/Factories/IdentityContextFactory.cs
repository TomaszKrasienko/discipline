using discipline.daily_trackers.infrastructure.IdentityContext.Abstractions;
using Microsoft.AspNetCore.Http;

namespace discipline.daily_trackers.infrastructure.IdentityContext.Factories;

internal sealed class IdentityContextFactory(
    IHttpContextAccessor httpContextAccessor) : IIdentityContextFactory
{
    public IIdentityContext Create()
        => new Internals.IdentityContext(httpContextAccessor);
}