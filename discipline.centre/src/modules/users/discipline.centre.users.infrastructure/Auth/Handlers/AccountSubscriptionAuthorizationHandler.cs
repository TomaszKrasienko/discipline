using discipline.centre.shared.infrastructure.Auth;
using discipline.centre.users.infrastructure.Auth.Claims;
using Microsoft.AspNetCore.Authorization;

namespace discipline.centre.users.infrastructure.Auth.Handlers;

internal sealed class AccountSubscriptionAuthorizationHandler(
    TimeProvider timeProvider) : AuthorizationHandler<AccountSubscriptionRequirement>
{
    //TODO: Should I test it?
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AccountSubscriptionRequirement requirement)
    {
        if (context is null)
        {
            throw new ArgumentException("Context can not be null");
        }

        var activeTillClaim = context
            .User
            .Claims
            .SingleOrDefault(x => x.Type == CustomClaimTypes.ActiveTill);
        
        var isPayedSubscriptionClaim = context
            .User
            .Claims
            .SingleOrDefault(x => x.Type == CustomClaimTypes.IsPayedSubscription);
        
        if (isPayedSubscriptionClaim is null)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        if (activeTillClaim?.Value is null && ! bool.Parse((ReadOnlySpan<char>)isPayedSubscriptionClaim.Value))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        if (activeTillClaim is null ||
            !DateOnly.TryParse(activeTillClaim.Value, out var activeTill))
        {
            context.Fail();
            return Task.CompletedTask;
        }

        var now = timeProvider.GetUtcNow();
        
        if (DateOnly.FromDateTime(now.DateTime) > activeTill)
        {
            context.Fail();
            return Task.CompletedTask;
        }
        
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}