using System.Collections.Generic;

namespace OrgChartDemo.Models.Repositories
{
    /// <summary>
    /// An implementation of <see cref="T:OrgChartDemo.Models.Repositories.IRepository{T}"/>
    /// </summary>
    /// <seealso cref="T:OrgChartDemo.Models.Repositories.IRepository{OrgChartDemo.Models.Position}" />
    public interface IPositionRepository : IRepository<Position>
    {
        /// <summary>
        /// Gets the Positions with their Members.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Position> GetPositionsWithMembers();
    }
}
