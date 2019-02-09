using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OrgChartDemo.Models;
using OrgChartDemo.Models.Repositories;
using OrgChartDemo.Models.Types;

namespace OrgChartDemo.Persistence.Repositories
{
    /// <summary>
    /// A repository for the MemberRace entity.
    /// </summary>
    /// <seealso cref="T:OrgChartDemo.Persistence.Repositories.Repository{OrgChartDemo.Models.Types.MemberRace}" />
    /// <seealso cref="T:OrgChartDemo.Models.Repositories.IMemberRaceRepository" />
    public class MemberRaceRepository : Repository<MemberRace>, IMemberRaceRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:OrgChartDemo.Persistence.Repositories.MemberRaceRepository"/> class.
        /// </summary>
        /// <param name="context">A <see cref="T:OrgChartDemo.Models.ApplicationDbContext"/></param>
        public MemberRaceRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Gets a list of <see cref="T:OrgChartDemo.Types.MemberRaceSelectListItem" />s.
        /// </summary>
        /// <remarks>
        /// This method is used to populate Rank select lists.
        /// </remarks>
        /// <returns>
        /// A <see cref="T:List{OrgChartDemo.Models.Types.MemberRaceSelectListItem}" />
        /// </returns>
        public List<MemberRaceSelectListItem> GetMemberRaceSelectListItems()
        {
            return GetAll().ToList().ConvertAll(x => new MemberRaceSelectListItem { MemberRaceId = x.MemberRaceId, RaceFullName = x.MemberRaceFullName, Abbreviation = x.Abbreviation });
        }

        public MemberRace GetRaceById(int memberRaceId)
        {
            return ApplicationDbContext.MemberRace
                .Where(x => x.MemberRaceId == memberRaceId)
                .FirstOrDefault();
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
