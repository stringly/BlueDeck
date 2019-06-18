using BlueDeck.Models.Types;
using System.Collections.Generic;
using BlueDeck.Models.Enums;

namespace BlueDeck.Models.Repositories
{
    /// <summary>
    /// An implementation of <see cref="T:BlueDeck.Models.Repositories.IRepository{T}"/>
    /// </summary>
    /// <seealso cref="T:BlueDeck.Models.Repositories.IRepository{BlueDeck.Models.Types.MemberDutyStatus}" />
    public interface IMemberDutyStatusRepository : IRepository<DutyStatus>
    {
        /// <summary>
        /// Gets a list of <see cref="T:BlueDeck.Types.MemberDutyStatusSelectListItem"/>s.
        /// </summary>
        /// <remarks>
        /// This method is used to populate Duty Status select lists.
        /// </remarks>
        /// <returns>A <see cref="T:List{BlueDeck.Models.Types.MemberDutyStatusSelectListItem}"/></returns>
        List<MemberDutyStatusSelectListItem> GetMemberDutyStatusSelectListItems();
        /// <summary>
        /// Gets the Duty Status with the given identifier.
        /// </summary>
        /// <param name="memberDutyStatus">The DutyStatusId of the desired Duty Status.</param>
        /// <returns>A <see cref="DutyStatus"/></returns>
        DutyStatus GetStatusById(int memberDutyStatus);
        /// <summary>
        /// Gets the a list of all current duty statuses with member count.
        /// </summary>
        /// <returns>A <see cref="List{DutyStatus}"/> of all current Duty Status types</returns>
        List<DutyStatus> GetDutyStatusesWithMemberCount();
        /// <summary>
        /// Gets a duty status with member count.
        /// </summary>
        /// <param name="id">The DutyStatusId of the desired Duty Status.</param>
        /// <returns>A <see cref="DutyStatus"/></returns>
        DutyStatus GetDutyStatusWithMemberCount(int id);
    }
}
