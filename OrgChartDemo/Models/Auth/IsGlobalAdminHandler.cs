using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace OrgChartDemo.Models.Auth
{
    public class IsGlobalAdminHandler : AuthorizationHandler<CanEditUserRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanEditUserRequirement requirement)
        {

            if (context.User.IsInRole("GlobalAdmin"))
            {
                context.Succeed(requirement);
            }
            return Task.FromResult(0);
        }
    }
}
