using BlueDeck.Models.Types;
using System.Collections.Generic;


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
        List<Gender> GetGendersWithMembers();
        Gender GetGenderById(int memberGenderId);
    }
}
