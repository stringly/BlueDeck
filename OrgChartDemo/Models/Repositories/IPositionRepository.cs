using OrgChartDemo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models.Repositories
{
    public interface IPositionRepository : IRepository<Position>
    {
        IEnumerable<Position> GetPositionsWithMemberCount();
    }
}
