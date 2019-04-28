using Microsoft.EntityFrameworkCore;
using OrgChartDemo.Models;
using OrgChartDemo.Models.Repositories;
using OrgChartDemo.Models.Types;
using System.Collections.Generic;
using System.Linq;


namespace OrgChartDemo.Persistence.Repositories
{
    /// <summary>
    /// A repository for the MemberRank entity.
    /// </summary>
    /// <seealso cref="T:OrgChartDemo.Persistence.Repositories.Repository{OrgChartDemo.Models.Types.MemberRank}" />
    /// <seealso cref="T:OrgChartDemo.Models.Repositories.IMemberRankRepository" />
    public class MemberRankRepository : Repository<Rank>, IMemberRankRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:OrgChartDemo.Persistence.Repositories.MemberRankRepository"/> class.
        /// </summary>
        /// <param name="context">A <see cref="T:OrgChartDemo.Models.ApplicationDbContext"/></param>
        public MemberRankRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets a list of <see cref="T:OrgChartDemo.Types.MemberRankSelectListItem" />s.
        /// </summary>
        /// <returns>
        /// A <see cref="T:List{OrgChartDemo.Models.Types.MemberRankSelectListItem}" />
        /// </returns>
        /// <remarks>
        /// This method is used to populate Rank select lists.
        /// </remarks>
        public List<MemberRankSelectListItem> GetMemberRankSelectListItems()
        {   
            return GetAll().ToList().ConvertAll(x => new MemberRankSelectListItem { MemberRankId = System.Convert.ToInt32(x.RankId), RankName = x.RankFullName });
        }

        public Rank GetRankById(int memberRankId)
        {
            return ApplicationDbContext.Ranks
                .Where(x => x.RankId == memberRankId)
                .FirstOrDefault();
        }

        public List<Rank> GetRanksWithMembers()
        {
            return ApplicationDbContext.Ranks.Include(x => x.Members).ToList();
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
