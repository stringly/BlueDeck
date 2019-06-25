using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace BlueDeck.Models.Auth.CanEditVehicle
{
    public class IsGlobalAdminForVehicleHandler : AuthorizationHandler<CanEditVehicleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanEditVehicleRequirement requirement)
        {
            if (context.User.IsInRole("GlobalAdmin"))
            {
                context.Succeed(requirement);
            }
            return Task.FromResult(0);
        }
    }
}
