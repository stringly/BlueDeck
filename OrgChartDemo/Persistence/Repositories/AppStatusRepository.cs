using OrgChartDemo.Models;
using OrgChartDemo.Models.Repositories;
using OrgChartDemo.Models.Types;
using System.Collections.Generic;
using System.Linq;

namespace OrgChartDemo.Persistence.Repositories
{
    public class AppStatusRepository : Repository<AppStatus>, IAppStatusRepository
    {
        public AppStatusRepository(ApplicationDbContext context) : base(context)
        {
        }
        public List<ApplicationStatusSelectListItem> GetApplicationStatusSelectListItems()
        {
            return GetAll().ToList().ConvertAll(x => new ApplicationStatusSelectListItem(x));
        }
    }
}
