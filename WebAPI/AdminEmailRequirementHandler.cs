using Microsoft.AspNetCore.Authorization;

namespace WebAPI;

public class AdminEmailRequirementHandler : AuthorizationHandler<AdminEmailRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminEmailRequirement requirement)
    {
        if (context.User.Identity?.Name == "admin@gmail.com")
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
