using OrgChartDemo.Models.Types;
using System.Collections.Generic;

namespace OrgChartDemo.Models.Repositories
{
    public interface IAppStatusRepository : IRepository<AppStatus>
    {
        List<ApplicationStatusSelectListItem> GetApplicationStatusSelectListItems();
        
    }
}

