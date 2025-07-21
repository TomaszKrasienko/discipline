using discipline.centre.shared.infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;

namespace discipline.centre.users.infrastructure.Users.Auth;

internal sealed class UserStateAuthorizationHandler() : AuthorizationHandler<UserStateRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserStateRequirement requirement)
    {
        if (context is null)
        {
            throw new ArgumentException("Context can not be null");
        }
        
        if (!(Ulid.TryParse(context.User?.Identity?.Name, out _)))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        var statusClaim = context.User?.Claims.SingleOrDefault(x => x.Type == CustomClaimTypes.Status);

        if (statusClaim is null)
        {
            context.Fail();
            return Task.CompletedTask;
        }
        
        context.Fail();
        return Task.CompletedTask;
    }
}