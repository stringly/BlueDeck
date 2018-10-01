using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OrgChartDemo.Models;
using OrgChartDemo.Models.Repositories;

namespace OrgChartDemo.Persistence.Repositories
{
    public class PositionRepository : Repository<Position>, IPositionRepository
    {
        public PositionRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public IEnumerable<Position> GetPositionsWithMembers()
        {
            return ApplicationDbContext.Positions.Include(c => c.Members).ToList();
        }

        public ApplicationDbContext ApplicationDbContext {
            get { return Context as ApplicationDbContext; }
        }
    }
}
