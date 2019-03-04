using Microsoft.EntityFrameworkCore;
using OrgChartDemo.Models;
using OrgChartDemo.Models.Repositories;
using OrgChartDemo.Models.Types;
using System.Collections.Generic;
using System.Linq;


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

        /// <summary>
        /// Gets the members with positions.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Member> GetMembersWithPositions()
        {
            return ApplicationDbContext.Members
                .Include(c => c.Rank)
                .Include(c => c.Position)
                .ThenInclude(c => c.ParentComponent)
                .ToList();
        }

        public Member GetMemberWithPosition(int memberId)
        {
            return ApplicationDbContext.Members
                .Where(x => x.MemberId == memberId)
                .Include(x => x.Position)
                    .ThenInclude(x => x.ParentComponent)
                .Include(x => x.Gender)
                .Include(x => x.Race)
                .Include(x => x.Rank)
                .Include(x => x.DutyStatus)                
                .FirstOrDefault();
        }

        public Member GetMemberWithDemographicsAndDutyStatus(int memberId)
        {
            return ApplicationDbContext.Members
                .Where(x => x.MemberId == memberId)
                .Include(x => x.PhoneNumbers)
                .Include(x => x.DutyStatus)
                .Include(x => x.Gender)
                .Include(x => x.Race)
                .Include(x => x.Rank)
                .FirstOrDefault();
        }

        public IEnumerable<MemberSelectListItem> GetAllMemberSelectListItems()
        {
            return ApplicationDbContext.Members
                .Include(x => x.Rank)
                .OrderByDescending(x => x.IdNumber)
                .ToList().ConvertAll(x => new MemberSelectListItem { MemberId = x.MemberId, MemberName = x.GetTitleName() });
        }

        /// <summary>
        /// Gets the application database context.
        /// </summary>
        /// <value>
        /// The application database context.
        /// </value>        
        public ApplicationDbContext ApplicationDbContext {
            get { return Context as ApplicationDbContext; }
        }
    }
}
