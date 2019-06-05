using BlueDeck.Models.Types;
using System.Collections.Generic;

namespace BlueDeck.Models.Repositories
{
    /// <summary>
    /// An implementation of <see cref="IRepository{Rank}"/> 
    /// </summary>
    /// <seealso cref="IRepository{Rank}" />
    public interface IMemberRankRepository : IRepository<Rank>
    {
        /// <summary>
        /// Gets a list of <see cref="MemberRankSelectListItem"/>s.
        /// </summary>
        /// <remarks>
        /// This method is used to populate Rank select lists.
        /// </remarks>
        /// <returns>A <see cref="MemberRankSelectListItem"/></returns>
        List<MemberRankSelectListItem> GetMemberRankSelectListItems();

        /// <summary>
        /// Gets the rank by identifier.
        /// </summary>
        /// <param name="memberRankId">The member rank identifier.</param>
        /// <returns>A <see cref="Rank"/> object.</returns>
        Rank GetRankById(int memberRankId);

        /// <summary>
        /// Gets the all ranks with their members.
        /// </summary>
        /// <returns>A <see cref="List{Rank}"/> of <see cref="Rank"/> with all of their <see cref="Member"/>s.</returns>
        List<Rank> GetRanksWithMembers();

        /// <summary>
        /// Gets a specific rank with its members.
        /// </summary>
        /// <param name="id">The identifier of the <see cref="Rank"/>.</param>
        /// <returns>A <see cref="Rank"/> with it's <see cref="Member"/>s.</returns>
        Rank GetRankWithMembers(int id);
    }
}
