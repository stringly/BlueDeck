using OrgChartDemo.Models.Types;
using System.Collections.Generic;


namespace OrgChartDemo.Models.Repositories
{
    /// <summary>
    /// An implementation of <see cref="T:OrgChartDemo.Models.Repositories.IRepository{T}"/>
    /// </summary>
    /// <seealso cref="T:OrgChartDemo.Models.Repositories.IRepository{OrgChartDemo.Models.Types.MemberRace}" />
    public interface IMemberRaceRepository: IRepository<MemberRace>
    {
        /// <summary>
        /// Gets a list of <see cref="T:OrgChartDemo.Types.MemberRaceSelectListItem"/>s.
        /// </summary>
        /// <remarks>
        /// This method is used to populate Rank select lists.
        /// </remarks>
        /// <returns>A <see cref="T:List{OrgChartDemo.Models.Types.MemberRaceSelectListItem}"/></returns>
        List<MemberRaceSelectListItem> GetMemberRaceSelectListItems();
        MemberRace GetRaceById(int memberRaceId);
    }
}
