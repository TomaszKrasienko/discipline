using discipline.centre.shared.infrastructure.Auth;
using discipline.centre.users.domain.Accounts.Enums;
using Microsoft.AspNetCore.Authorization;

namespace discipline.centre.users.infrastructure.Auth;

internal sealed class UserStateAuthorizationHandler() : AuthorizationHandler<UserStateRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserStateRequirement requirement)
    {
        if (context is null)
        {
            throw new ArgumentException("Context can not be null");
        }

        var statusClaim = context.User.Claims.SingleOrDefault(x 
            => x.Type == CustomClaimTypes.Status);

        if (statusClaim is null)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        if (statusClaim.Value == AccountState.Active.Value)
        {
            context.Succeed(requirement);
        }
        
        return Task.CompletedTask;
    }
}