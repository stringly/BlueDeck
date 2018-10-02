using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OrgChartDemo.Models;
using OrgChartDemo.Models.Repositories;
using OrgChartDemo.Models.Types;


namespace OrgChartDemo.Persistence.Repositories
{
    public class MemberRankRepository : Repository<MemberRank>, IMemberRankRepository
    {
        public MemberRankRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public List<MemberRankSelectListItem> GetMemberRankSelectListItems()
        {            
            return GetAll().ToList().ConvertAll(x => new MemberRankSelectListItem { MemberRankId = x.RankId, RankName = x.RankFullName });
        }
    }

   
}
