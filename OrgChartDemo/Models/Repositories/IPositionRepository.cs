using System.Collections.Generic;

namespace OrgChartDemo.Models.Repositories
{
    public interface IPositionRepository : IRepository<Position>
    {
        IEnumerable<Position> GetPositionsWithMembers();
    }
}
