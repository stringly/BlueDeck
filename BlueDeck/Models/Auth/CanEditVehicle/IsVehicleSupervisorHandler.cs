using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using BlueDeck.Models.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.Auth.CanEditVehicle
{
    public class IsVehicleSupervisorHandler : AuthorizationHandler<CanEditVehicleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanEditVehicleRequirement requirement)
        {
            if (context.User.IsInRole("ComponentAdmin"))
            {
                if (context.User.HasClaim(claim => claim.Type == "CanEditVehicles"))
                {
                    List<VehicleSelectListItem> vehicles =
                        JsonConvert.DeserializeObject<List<VehicleSelectListItem>>(
                            context.User.Claims.FirstOrDefault(claim => claim.Type == "CanEditVehicles")
                            .Value
                            .ToString());

                    var authContext = (AuthorizationFilterContext)context.Resource;
                    var routeComponentId = Convert.ToInt32(authContext.HttpContext.GetRouteValue("id")?.ToString() ?? null);
                    if (vehicles.Any(x => x.VehicleId == routeComponentId))
                    {
                        context.Succeed(requirement);
                    }

                }
            }
            return Task.FromResult(0);
        }
    }
}
