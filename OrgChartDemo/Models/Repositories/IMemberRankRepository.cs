using OrgChartDemo.Models.Types;
using System.Collections.Generic;

namespace OrgChartDemo.Models.Repositories
{
    /// <summary>
    /// An implementation of <see cref="T:OrgChartDemo.Models.Repositories.IRepository{T}"/>
    /// </summary>
    /// <seealso cref="T:OrgChartDemo.Models.Repositories.IRepository{OrgChartDemo.Models.MemberRank}" />
    public interface IMemberRankRepository : IRepository<MemberRank>
    {
        /// <summary>
        /// Gets a list of <see cref="T:OrgChartDemo.Types.MemberRankSelectListItem"/>s.
        /// </summary>
        /// <remarks>
        /// This method is used to populate Rank select lists.
        /// </remarks>
        /// <returns>A <see cref="T:List{OrgChartDemo.Models.Types.MemberRankSelectListItem}"/></returns>
        List<MemberRankSelectListItem> GetMemberRankSelectListItems();
    }
}
