using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OrgChartDemo.Models;
using OrgChartDemo.Models.Types;
using OrgChartDemo.Models.Repositories;


namespace OrgChartDemo.Persistence.Repositories
{
    /// <summary>
    /// A repository for the Member entity
    /// </summary>
    /// <seealso cref="T:OrgChartDemo.Persistence.Repositories.Repository{OrgChartDemo.Models.Member}" />
    /// <seealso cref="T:OrgChartDemo.Models.Repositories.IMemberRepository" />
    public class MemberRepository : Repository<Member>, IMemberRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:OrgChartDemo.Persistence.Repositories.MemberRepository"/> class.
        /// </summary>
        /// <param name="context">The <see cref="T:OrgChartDemo.Models.ApplicationDbContext"/>.</param>
        public MemberRepository(ApplicationDbContext context)
         : base(context)
        {
        }
    }
}
