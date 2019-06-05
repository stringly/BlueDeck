using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BlueDeck.Models;
using BlueDeck.Models.Repositories;
using BlueDeck.Models.Types;

namespace BlueDeck.Persistence.Repositories
{
    /// <summary>
    /// A repository for the MemberRace entity.
    /// </summary>
    /// <seealso cref="T:BlueDeck.Persistence.Repositories.Repository{BlueDeck.Models.Types.MemberRace}" />
    /// <seealso cref="T:BlueDeck.Models.Repositories.IMemberRaceRepository" />
    public class MemberRaceRepository : Repository<Race>, IMemberRaceRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:BlueDeck.Persistence.Repositories.MemberRaceRepository"/> class.
        /// </summary>
        /// <param name="context">A <see cref="T:BlueDeck.Models.ApplicationDbContext"/></param>
        public MemberRaceRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Gets a list of <see cref="T:BlueDeck.Types.MemberRaceSelectListItem" />s.
        /// </summary>
        /// <remarks>
        /// This method is used to populate Rank select lists.
        /// </remarks>
        /// <returns>
        /// A <see cref="T:List{BlueDeck.Models.Types.MemberRaceSelectListItem}" />
        /// </returns>
        public List<MemberRaceSelectListItem> GetMemberRaceSelectListItems()
        {
            return GetAll().ToList().ConvertAll(x => new MemberRaceSelectListItem { MemberRaceId = System.Convert.ToInt32(x.MemberRaceId), RaceFullName = x.MemberRaceFullName, Abbreviation = x.Abbreviation });
        }

        /// <summary>
        /// Gets the race by identifier.
        /// </summary>
        /// <param name="memberRaceId">The member race identifier.</param>
        /// <returns>A <see cref="Race"/></returns>
        public Race GetRaceById(int memberRaceId)
        {
            return ApplicationDbContext.Races
                .Where(x => x.MemberRaceId == memberRaceId)
                .FirstOrDefault();
        }

        /// <summary>
        /// Gets a lsit of all Races including their members.
        /// </summary>
        /// <returns>
        /// A <see cref="List{Member}" /> of all races including their members.
        /// </returns>
        public List<Race> GetRacesWithMembers()
        {
            return ApplicationDbContext.Races.Include(x => x.Members).ToList();
        }

        /// <summary>
        /// Gets a race with it's members.
        /// </summary>
        /// <param name="id">The identifier of the <see cref="Race" />.</param>
        /// <returns></returns>
        public Race GetRaceWithMembers(int id)
        {
            return ApplicationDbContext.Races.Include(x => x.Members).First(x => x.MemberRaceId == id);
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
