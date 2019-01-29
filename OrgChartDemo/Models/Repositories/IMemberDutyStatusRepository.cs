using OrgChartDemo.Models.Types;
using System.Collections.Generic;

namespace OrgChartDemo.Models.Repositories
{
    /// <summary>
    /// An implementation of <see cref="T:OrgChartDemo.Models.Repositories.IRepository{T}"/>
    /// </summary>
    /// <seealso cref="T:OrgChartDemo.Models.Repositories.IRepository{OrgChartDemo.Models.Types.MemberDutyStatus}" />
    public interface IMemberDutyStatusRepository : IRepository<MemberDutyStatus>
    {
        /// <summary>
        /// Gets a list of <see cref="T:OrgChartDemo.Types.MemberDutyStatusSelectListItem"/>s.
        /// </summary>
        /// <remarks>
        /// This method is used to populate Duty Status select lists.
        /// </remarks>
        /// <returns>A <see cref="T:List{OrgChartDemo.Models.Types.MemberDutyStatusSelectListItem}"/></returns>
        List<MemberDutyStatusSelectListItem> GetMemberDutyStatusSelectListItems();
    }
}
