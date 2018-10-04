using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OrgChartDemo.Models;
using OrgChartDemo.Models.Repositories;

namespace OrgChartDemo.Persistence.Repositories
{
    /// <summary>
    /// A repository for the Position entity
    /// </summary>
    /// <seealso cref="T:OrgChartDemo.Persistence.Repositories.Repository{OrgChartDemo.Models.Position}" />
    /// <seealso cref="T:OrgChartDemo.Models.Repositories.IPositionRepository" />
    public class PositionRepository : Repository<Position>, IPositionRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:OrgChartDemo.Persistence.Repositories.PositionRepository"/> class.
        /// </summary>
        /// <param name="context">An <see cref="T:OrgChartDemo.Models.ApplicationDbContext"/>.</param>
        public PositionRepository(ApplicationDbContext context)
            : base(context)
        {            
        }

        /// <summary>
        /// Gets the Positions with their Members.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Position> GetPositionsWithMembers()
        {
            return ApplicationDbContext.Positions.Include(c => c.Members).Include(c => c.ParentComponent).ToList();
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
