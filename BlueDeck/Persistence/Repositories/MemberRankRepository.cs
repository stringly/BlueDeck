using Microsoft.EntityFrameworkCore;
using BlueDeck.Models;
using BlueDeck.Models.Repositories;
using BlueDeck.Models.Types;
using System.Collections.Generic;
using System.Linq;


namespace BlueDeck.Persistence.Repositories
{
    /// <summary>
    /// A repository for the MemberRank entity.
    /// </summary>
    /// <seealso cref="T:BlueDeck.Persistence.Repositories.Repository{BlueDeck.Models.Types.MemberRank}" />
    /// <seealso cref="T:BlueDeck.Models.Repositories.IMemberRankRepository" />
    public class MemberRankRepository : Repository<Rank>, IMemberRankRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:BlueDeck.Persistence.Repositories.MemberRankRepository"/> class.
        /// </summary>
        /// <param name="context">A <see cref="T:BlueDeck.Models.ApplicationDbContext"/></param>
        public MemberRankRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets a list of <see cref="T:BlueDeck.Types.MemberRankSelectListItem" />s.
        /// </summary>
        /// <returns>
        /// A <see cref="T:List{BlueDeck.Models.Types.MemberRankSelectListItem}" />
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
