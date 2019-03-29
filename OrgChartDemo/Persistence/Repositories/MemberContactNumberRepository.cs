using OrgChartDemo.Models;
using OrgChartDemo.Models.Repositories;
using OrgChartDemo.Models.Types;

namespace OrgChartDemo.Persistence.Repositories
{
    public class MemberContactNumberRepository : Repository<ContactNumber>, IMemberContactNumberRepository
    {
        public MemberContactNumberRepository(ApplicationDbContext context) : base(context)
        {
        }
        public ApplicationDbContext ApplicationDbContext {
            get { return Context as ApplicationDbContext; }
        }
    }
}
