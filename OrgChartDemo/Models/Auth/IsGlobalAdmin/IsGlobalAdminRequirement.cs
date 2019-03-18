using Microsoft.AspNetCore.Authorization;

namespace OrgChartDemo.Models.Auth
{
    public class IsGlobalAdminRequirement: IAuthorizationRequirement
    {
    }
    
}
