using BlueDeck.Models.Types;
using System.Collections.Generic;


namespace BlueDeck.Models.Repositories
{
    /// <summary>
    /// An implementation of <see cref="T:BlueDeck.Models.Repositories.IRepository{T}"/>
    /// </summary>
    /// <seealso cref="T:BlueDeck.Models.Repositories.IRepository{BlueDeck.Models.Types.MemberRace}" />
    public interface IMemberRaceRepository: IRepository<Race>
    {
        /// <summary>
        /// Gets a list of <see cref="T:BlueDeck.Types.MemberRaceSelectListItem"/>s.
        /// </summary>
        /// <remarks>
        /// This method is used to populate Rank select lists.
        /// </remarks>
        /// <returns>A <see cref="T:List{BlueDeck.Models.Types.MemberRaceSelectListItem}"/></returns>
        List<MemberRaceSelectListItem> GetMemberRaceSelectListItems();
        Race GetRaceById(int memberRaceId);
        List<Race> GetRacesWithMembers();
    }
}
