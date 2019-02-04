using OrgChartDemo.Models.Types;
using System.Collections.Generic;

namespace OrgChartDemo.Models.Repositories
{
    /// <summary>
    /// An implementation of <see cref="T:OrgChartDemo.Models.Repositories.IRepository{T}"/>
    /// </summary>
    /// <seealso cref="T:OrgChartDemo.Models.Repositories.IRepository{OrgChartDemo.Models.Member}" />
    public interface IMemberRepository : IRepository<Member>
    {
        IEnumerable<Member> GetMembersWithPositions();
        Member GetMemberWithPosition(int memberId);
    }
}
