using BlueDeck.Models.APIModels;
using BlueDeck.Models.Types;
using BlueDeck.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueDeck.Models.Repositories
{
    /// <summary>
    /// An implementation of <see cref="T:BlueDeck.Models.Repositories.IRepository{T}"/>
    /// </summary>
    /// <seealso cref="T:BlueDeck.Models.Repositories.IRepository{BlueDeck.Models.Position}" />
    public interface IPositionRepository : IRepository<Position>
    {
        /// <summary>
        /// Gets the Positions with their Members.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Position> GetPositionsWithMembers();

        List<PositionSelectListItem> GetAllPositionSelectListItems();
        IEnumerable<PositionSelectListItem> GetUnoccupiedAndNonUniquePositionSelectListItems();
        Position GetPositionAndAllCurrentMembers(int positionId);
        /// <summary>
        /// Removes the position and reassign members.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="newPositionName">New name of the position.</param>
        void RemovePositionAndReassignMembers(int id, string newPositionName = "Unassigned");
        Position GetPositionWithParentComponent(int positionId);
        void UpdatePositionAndSetLineup(Position p);
        List<PositionSelectListItem> GetPositionsUserCanEdit(int componentId);
        AdminPositionIndexListViewModel GetAdminPositionIndexListViewModel();
        Task<PositionApiResult> GetApiPosition(int id);

        /// <summary>
        /// Gets the position with vehicles.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Position GetPositionWithVehicles(int id);
    }
}
