using OrgChartDemo.Models.Types;
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

        IEnumerable<PositionSelectListItem> GetAllPositionSelectListItems();
        IEnumerable<PositionSelectListItem> GetUnoccupiedAndNonUniquePositionSelectListItems();
        Position GetPositionAndAllCurrentMembers(int positionId);
        /// <summary>
        /// Removes the position and reassign members.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="newPositionName">New name of the position.</param>
        void RemovePositionAndReassignMembers(int id, string newPositionName = "Unassigned");
        Position GetPositionWithParentComponent(int positionId);
    }
}
