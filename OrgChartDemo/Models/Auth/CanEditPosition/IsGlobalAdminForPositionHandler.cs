using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace OrgChartDemo.Models.Auth
{
    public class IsGlobalAdminForPositionHandler : AuthorizationHandler<CanEditPositionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanEditPositionRequirement requirement)
        {
            if (context.User.IsInRole("GlobalAdmin"))
            {
                context.Succeed(requirement);
            }
            return Task.FromResult(0);
        }
    }
}
