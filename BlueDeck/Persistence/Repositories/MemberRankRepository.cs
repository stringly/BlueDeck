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
            return GetAll().ToList().ConvertAll(x => new MemberRankSelectListItem(x));
        }

        /// <summary>
        /// Gets the rank by identifier.
        /// </summary>
        /// <param name="memberRankId">The member rank identifier.</param>
        /// <returns>
        /// A <see cref="Rank" /> object.
        /// </returns>
        public Rank GetRankById(int memberRankId)
        {
            return ApplicationDbContext.Ranks
                .Where(x => x.RankId == memberRankId)
                .FirstOrDefault();
        }

        /// <summary>
        /// Gets a list of all ranks with their members.
        /// </summary>
        /// <returns>
        /// A <see cref="List{Rank}"/> of <see cref="Rank" /> with all of their <see cref="Member" />s.
        /// </returns>
        public List<Rank> GetRanksWithMembers()
        {
            return ApplicationDbContext.Ranks.Include(x => x.Members).ToList();
        }

        /// <summary>
        /// Gets a specific rank with its members.
        /// </summary>
        /// <param name="id">The identifier of the <see cref="Rank" />.</param>
        /// <returns>
        /// A <see cref="Rank" /> with it's <see cref="Member" />s.
        /// </returns>
        public Rank GetRankWithMembers(int id)
        {
            return ApplicationDbContext.Ranks.Include(x => x.Members).First(x => x.RankId == id);
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
