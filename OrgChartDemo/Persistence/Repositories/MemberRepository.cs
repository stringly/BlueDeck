using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OrgChartDemo.Models;
using OrgChartDemo.Models.Types;
using OrgChartDemo.Models.Repositories;


namespace OrgChartDemo.Persistence.Repositories
{
    public class MemberRepository : Repository<Member>, IMemberRepository
    {
        public MemberRepository(ApplicationDbContext context)
         : base(context)
        {
        }
    }
}
