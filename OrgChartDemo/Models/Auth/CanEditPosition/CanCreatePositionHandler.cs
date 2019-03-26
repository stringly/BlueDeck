using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace OrgChartDemo.Models.Auth
{
    public class CanCreatePositionHandler : AuthorizationHandler<CanEditPositionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanEditPositionRequirement requirement)
        {
            if (context.User.IsInRole("ComponentAdmin"))
            {
                if (context.User.HasClaim(claim => claim.Type == "CanEditPositions"))
                {
                    context.Succeed(requirement);                   
                }
            }
            return Task.FromResult(0);
        }
    }
}
