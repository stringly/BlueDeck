using OrgChartDemo.Models.Types;
using System.Collections.Generic;


namespace OrgChartDemo.Models.Repositories
{
    /// <summary>
    /// An implementation of <see cref="T:OrgChartDemo.Models.Repositories.IRepository{T}"/>
    /// </summary>
    /// <seealso cref="T:OrgChartDemo.Models.Repositories.IRepository{OrgChartDemo.Models.Types.MemberGender}" />
    public interface IMemberGenderRepository : IRepository<Gender>
    {
        /// <summary>
        /// Gets a list of <see cref="T:OrgChartDemo.Types.MemberGenderSelectListItem"/>s.
        /// </summary>
        /// <remarks>
        /// This method is used to populate Gender select lists.
        /// </remarks>
        /// <returns>A <see cref="T:List{OrgChartDemo.Models.Types.MemberGenderSelectListItem}"/></returns>
        List<MemberGenderSelectListItem> GetMemberGenderSelectListItems();
        List<Gender> GetGendersWithMembers();
        Gender GetGenderById(int memberGenderId);
    }
}
