using BlueDeck.Models.Types;
using System.Collections.Generic;

namespace BlueDeck.Models.Repositories
{
    /// <summary>
    /// An implementation of <see cref="T:BlueDeck.Models.Repositories.IRepository{T}"/>
    /// </summary>
    /// <seealso cref="T:BlueDeck.Models.Repositories.IRepository{BlueDeck.Models.MemberRank}" />
    public interface IMemberRankRepository : IRepository<Rank>
    {
        /// <summary>
        /// Gets a list of <see cref="T:BlueDeck.Types.MemberRankSelectListItem"/>s.
        /// </summary>
        /// <remarks>
        /// This method is used to populate Rank select lists.
        /// </remarks>
        /// <returns>A <see cref="T:List{BlueDeck.Models.Types.MemberRankSelectListItem}"/></returns>
        List<MemberRankSelectListItem> GetMemberRankSelectListItems();
        Rank GetRankById(int memberRankId);
        List<Rank> GetRanksWithMembers();
    }
}
