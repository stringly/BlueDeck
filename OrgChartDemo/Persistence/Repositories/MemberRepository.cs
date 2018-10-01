using OrgChartDemo.Models;
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
