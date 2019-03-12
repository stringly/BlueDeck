using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

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

            // TODO: pull user roles 
            // var dbUser = _unitOfWork.Members.GetUserWithRoles(user);

            var ci = (ClaimsIdentity) principal.Identity;
            var c = new Claim(ci.RoleClaimType, "Admin");
            ci.AddClaim(c);
            return principal;
        }
    }
}
