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
    public class IsMemberSupervisorHandler : AuthorizationHandler<CanEditUserRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanEditUserRequirement requirement)
        {
            if (context.User.IsInRole("ComponentAdmin"))
            {
                if (context.User.HasClaim(claim => claim.Type == "CanEditUsers"))
                {
                    List<MemberSelectListItem> members = JsonConvert.DeserializeObject<List<MemberSelectListItem>>(context.User.Claims.FirstOrDefault(claim => claim.Type == "CanEditUsers").Value.ToString());
                    
                    //retrieve the ID of the Member the user is trying to Edit from the url route
                    var authContext = (AuthorizationFilterContext)context.Resource;
                    var routeMemberId = Convert.ToInt32(authContext.HttpContext.GetRouteValue("id").ToString());
                    
                    if (members.Any(x => x.MemberId == routeMemberId))
                    {
                        context.Succeed(requirement);
                    }
                    
                }
            }
            return Task.FromResult(0);
        }
    }
}
