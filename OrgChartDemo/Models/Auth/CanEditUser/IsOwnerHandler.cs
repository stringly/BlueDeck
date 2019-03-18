using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models.Auth
{
    public class IsOwnerHandler : AuthorizationHandler<CanEditUserRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanEditUserRequirement requirement)
        {
            if(context.User.HasClaim(claim => claim.Type == "MemberId"))
            {
                var claimMemberId = context.User.Claims.FirstOrDefault(claim => claim.Type == "MemberId").Value.ToString();
                var authContext = (AuthorizationFilterContext)context.Resource;
                var routeMemberId = authContext.HttpContext.GetRouteValue("id").ToString();
                if (claimMemberId == routeMemberId)
                {
                    context.Succeed(requirement);
                }
            }
            return Task.FromResult(0);
        }
    }
}
