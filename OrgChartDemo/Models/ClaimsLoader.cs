using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using OrgChartDemo.Models.Types;

namespace OrgChartDemo.Models
{
    public class ClaimsLoader : IClaimsTransformation
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClaimsLoader(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)        
        {
            var identity = principal.Identities.FirstOrDefault(x => x.IsAuthenticated);
            if(identity == null) return principal;

            var user = identity.Name;
            // Is this the Windows Logon name?
            if (user == null) return principal;

            if(principal.Identity is ClaimsIdentity)
            {
                string logonName = user.Split('\\')[1];
                // pull user roles 
                Member dbUser = _unitOfWork.Members.GetMemberWithRoles(logonName);
                if (dbUser != null)
                {
                    var ci = (ClaimsIdentity)principal.Identity;
                    foreach (UserRole ur in dbUser.CurrentRoles)
                    {
                        var c = new Claim(ci.RoleClaimType, ur.RoleType.RoleTypeName);
                        ci.AddClaim(c);
                        if (ur.RoleType.RoleTypeName == "ComponentAdmin")
                        {
                            int memberParentComponentId = dbUser.Position.ParentComponent.ComponentId;
                            // TODO: Repo method to get tree of componentIds for the user's parent component
                            List<ComponentSelectListItem> canEditComponents = _unitOfWork.Components.GetChildComponentsForComponentId(memberParentComponentId);
                            var d = new Claim("CanEditComponents", JsonConvert.SerializeObject(canEditComponents));
                            ci.AddClaim(d);
                            List<MemberSelectListItem> canEditMembers = _unitOfWork.Members.GetMembersUserCanEdit(memberParentComponentId);
                            var e = new Claim("CanEditUsers", JsonConvert.SerializeObject(canEditMembers));
                            ci.AddClaim(e);
                            List<PositionSelectListItem> canEditPositions = _unitOfWork.Positions.GetPositionsUserCanEdit(memberParentComponentId);
                            var f = new Claim("CanEditPositions", JsonConvert.SerializeObject(canEditPositions));
                            ci.AddClaim(f);
                        }
                    }
                    ci.AddClaim(new Claim(ClaimTypes.GivenName, dbUser.FirstName));
                    ci.AddClaim(new Claim(ClaimTypes.Surname, dbUser.LastName));
                    ci.AddClaim(new Claim("MemberId", dbUser.MemberId.ToString(), ClaimValueTypes.Integer32));
                    ci.AddClaim(new Claim("DisplayName", dbUser.GetTitleName()));
                }
                else
                {
                    var ci = (ClaimsIdentity)principal.Identity;
                    ci.AddClaim(new Claim("DisplayName", "Guest"));
                    ci.AddClaim(new Claim("MemberId", "0", ClaimValueTypes.Integer32));
                }
                
            }
                        
            return principal;
        }
    }
}
