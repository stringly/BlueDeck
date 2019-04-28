using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace BlueDeck.Models.Auth
{
    public class IsGlobalAdminForComponentHandler : AuthorizationHandler<CanEditComponentRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanEditComponentRequirement requirement)
        {
            if (context.User.IsInRole("GlobalAdmin"))
            {
                context.Succeed(requirement);
            }
            return Task.FromResult(0);
        }
    }
}