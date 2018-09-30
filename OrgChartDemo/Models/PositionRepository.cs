using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrgChartDemo.Models.Repositories;
using OrgChartDemo.Persistence.Repositories;

namespace OrgChartDemo.Models
{
    public class PositionRepository : Repository<Position>, IPositionRepository
    {
        public PositionRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public IEnumerable<Position> GetPositionsWithMemberCount()
        {
            return ApplicationDbContext.Positions.Include(c => c.Members).ToList();
        }
    }
}
