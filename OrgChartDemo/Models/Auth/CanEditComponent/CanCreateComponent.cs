using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace OrgChartDemo.Models.Auth
{
    public class CanCreateComponent : AuthorizationHandler<CanEditComponentRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanEditComponentRequirement requirement)
        {
            if (context.User.IsInRole("ComponentAdmin"))
            {
                if (context.User.HasClaim(claim => claim.Type == "CanEditComponents"))
                {                   
                    context.Succeed(requirement);
                }
            }
            return Task.FromResult(0);
        }
    }
}
