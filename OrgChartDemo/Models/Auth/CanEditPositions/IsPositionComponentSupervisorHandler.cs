using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using OrgChartDemo.Models.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace OrgChartDemo.Models.Auth
{
    public class IsPositionComponentSupervisorHandler : AuthorizationHandler<CanEditPositionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanEditPositionRequirement requirement)
        {
            if (context.User.IsInRole("ComponentAdmin"))
            {
                if(context.User.HasClaim(claim => claim.Type == "CanEditPositions"))
                {
                    List<PositionSelectListItem> positions = JsonConvert.DeserializeObject<List<PositionSelectListItem>>(context.User.Claims.FirstOrDefault(claim => claim.Type == "CanEditPositions").Value.ToString());
                    var authContext = (AuthorizationFilterContext)context.Resource;
                    var routeComponentId = Convert.ToInt32(authContext.HttpContext.GetRouteValue("id").ToString());
                    if (positions.Any(x => x.PositionId == routeComponentId))
                    {
                        context.Succeed(requirement);
                    }
                }
            }
            return Task.FromResult(0);
        }
    }
}
