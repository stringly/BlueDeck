using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.Auth.CanEditVehicle
{
    public class IsVehicleOwnerHandler : AuthorizationHandler<CanEditVehicleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanEditVehicleRequirement requirement)
        {
            if (context.User.HasClaim(claim => claim.Type == "VehicleId"))
            {
                var claimMemberId = context.User.Claims.FirstOrDefault(claim => claim.Type == "VehicleId").Value.ToString();
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
