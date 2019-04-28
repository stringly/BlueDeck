using BlueDeck.Models.Types;
using System.Collections.Generic;

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
        
        DutyStatus GetStatusById(int memberDutyStatus);
        List<DutyStatus> GetDutyStatusesWithMemberCount();
    }
}
