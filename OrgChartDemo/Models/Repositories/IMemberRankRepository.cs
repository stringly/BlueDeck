using OrgChartDemo.Models.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models.Repositories
{
    public interface IMemberRankRepository : IRepository<MemberRank>
    {
        List<MemberRankSelectListItem> GetMemberRankSelectListItems();
    }
}
