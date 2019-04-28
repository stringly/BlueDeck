using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;


namespace OrgChartDemo.Models.Auth
{
    public class IsGlobalAdmin : AuthorizationHandler<IsGlobalAdminRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsGlobalAdminRequirement requirement)
        {
            if (context.User.IsInRole("GlobalAdmin"))
            {
                context.Succeed(requirement);
            }
            return Task.FromResult(0);
        }
    }
}
