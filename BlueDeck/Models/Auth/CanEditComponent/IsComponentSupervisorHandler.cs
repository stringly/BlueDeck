using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using BlueDeck.Models.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.Auth
{
    public class IsComponentSupervisorHandler : AuthorizationHandler<CanEditComponentRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanEditComponentRequirement requirement)
        {
            if (context.User.IsInRole("ComponentAdmin"))
            {
                if(context.User.HasClaim(claim => claim.Type == "CanEditComponents"))
                {
                    List<ComponentSelectListItem> components = 
                        JsonConvert.DeserializeObject<List<ComponentSelectListItem>>(
                            context.User.Claims.FirstOrDefault(claim => claim.Type == "CanEditComponents")
                            .Value
                            .ToString());

                    var authContext = (AuthorizationFilterContext)context.Resource;
                    var routeComponentId = Convert.ToInt32(authContext.HttpContext.GetRouteValue("id")?.ToString() ?? null);
                    if (components.Any(x => x.Id == routeComponentId))
                    {
                        context.Succeed(requirement);
                    }
                    
                }
            }
            return Task.FromResult(0);
        }
    }
}
