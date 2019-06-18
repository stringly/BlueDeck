using BlueDeck.Models.Types;
using System.Collections.Generic;
using BlueDeck.Models.Enums;


namespace BlueDeck.Models.Repositories
{
    /// <summary>
    /// An implementation of <see cref="IRepository{Race}"/> that handles database actions for the <see cref="Race"/> entity.
    /// </summary>
    /// <seealso cref="IRepository{Race}" />
    public interface IMemberRaceRepository: IRepository<Race>
    {
        /// <summary>
        /// Gets a list of <see cref="MemberRaceSelectListItem"/>s.
        /// </summary>
        /// <remarks>
        /// This method is used to populate Rank select lists.
        /// </remarks>
        /// <returns>A <see cref="List{MemberRaceSelectListItem}"/></returns>
        List<MemberRaceSelectListItem> GetMemberRaceSelectListItems();

        /// <summary>
        /// Gets the race by identifier.
        /// </summary>
        /// <param name="memberRaceId">The member race identifier.</param>
        /// <returns></returns>
        Race GetRaceById(int memberRaceId);

        /// <summary>
        /// Gets a lsit of all Races including their members.
        /// </summary>
        /// <returns>A <see cref="List{Member}"/> of all races including their members.</returns>
        List<Race> GetRacesWithMembers();

        /// <summary>
        /// Gets a race with it's members.
        /// </summary>
        /// <param name="id">The identifier of the <see cref="Race"/>.</param>
        /// <returns></returns>
        Race GetRaceWithMembers(int id);
    }
}
