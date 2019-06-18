using BlueDeck.Models.Types;
using System.Collections.Generic;
using BlueDeck.Models.Enums;


namespace BlueDeck.Models.Repositories
{
    /// <summary>
    /// An implementation of <see cref="T:BlueDeck.Models.Repositories.IRepository{T}"/>
    /// </summary>
    /// <seealso cref="T:BlueDeck.Models.Repositories.IRepository{BlueDeck.Models.Types.MemberGender}" />
    public interface IMemberGenderRepository : IRepository<Gender>
    {
        /// <summary>
        /// Gets a list of <see cref="T:BlueDeck.Types.MemberGenderSelectListItem"/>s.
        /// </summary>
        /// <remarks>
        /// This method is used to populate Gender select lists.
        /// </remarks>
        /// <returns>A <see cref="T:List{BlueDeck.Models.Types.MemberGenderSelectListItem}"/></returns>
        List<MemberGenderSelectListItem> GetMemberGenderSelectListItems();
        /// <summary>
        /// Gets a list of all <see cref="Gender"/> with members.
        /// </summary>
        /// <returns>A <see cref="List{Gender}"/> containing all current <see cref="Gender"/></returns>
        List<Gender> GetGendersWithMembers();
        /// <summary>
        /// Gets a <see cref="Gender"/> with it's current <see cref="Member"/>s.
        /// </summary>
        /// <param name="id">The identifier of the <see cref="Gender"/>.</param>
        /// <returns>A <see cref="Gender"/></returns>
        Gender GetGenderWithMembers(int id);

        /// <summary>
        /// Gets the gender by identifier.
        /// </summary>
        /// <param name="memberGenderId">The <see cref="Gender"/> identifier.</param>
        /// <returns>A <see cref="Gender"/></returns>
        Gender GetGenderById(int memberGenderId);
    }
}
