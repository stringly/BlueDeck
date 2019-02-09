using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OrgChartDemo.Models;
using OrgChartDemo.Models.Repositories;
using OrgChartDemo.Models.Types;

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
            return ApplicationDbContext.Positions.Include(c => c.Members)
                .ThenInclude(c => c.Rank)
                .Include(c => c.ParentComponent)
                .Where(c => c.ParentComponent != null)
                .ToList();
        }

        public IEnumerable<PositionSelectListItem> GetAllPositionSelectListItems(){
            return GetAll().ToList().ConvertAll(x => new PositionSelectListItem { PositionId = x.PositionId, PositionName = x.Name});
        }

        public IEnumerable<PositionSelectListItem> GetUnoccupiedAndNonUniquePositionSelectListItems()
        {
            return ApplicationDbContext.Positions
                        .Include(x => x.Members)
                        .Where(x => x.IsUnique == false || x.Members.Count() == 0).ToList()
                        .ConvertAll(x => new PositionSelectListItem { PositionId = x.PositionId, PositionName = x.Name});
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

        /// <summary>
        /// Removes the position and reassigns any members.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="newPositionName">New name of the position.</param>
        public void RemovePositionAndReassignMembers(int id, string newPositionName = "Unassigned")
        {
            Position toDelete = ApplicationDbContext.Positions.Include(x => x.Members).First(x => x.PositionId == id);
            Position toReassign = ApplicationDbContext.Positions.First(x => x.Name == newPositionName);
            toReassign.Members.AddRange(toDelete.Members);            
            ApplicationDbContext.Positions.Remove(toDelete);
        }
        public Position GetPositionWithParentComponent(int positionId)
        {
            return ApplicationDbContext.Positions
                .Include(x => x.ParentComponent)
                .Where(x => x.PositionId == positionId)
                .FirstOrDefault();
        }
    }
}
