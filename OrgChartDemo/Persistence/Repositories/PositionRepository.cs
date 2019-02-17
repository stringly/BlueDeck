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
        /// Gets the application database context.
        /// </summary>
        /// <value>
        /// The application database context.
        /// </value>
        public ApplicationDbContext ApplicationDbContext {
            get { return Context as ApplicationDbContext; }
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
       
        public Position GetPositionAndAllCurrentMembers(int positionId)
        {
            return ApplicationDbContext.Positions.Where(x => x.PositionId == positionId).Include(x => x.Members).SingleOrDefault();
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

        public void UpdatePositionAndSetLineup(Position p)
        {
            // assume that the Position's ParentComponent is set? It has to be, right? Hmmm...
            // I don't assume that the Position's ParentComponent's Child Positions collection will be set...
            p.ParentComponent.Positions = ApplicationDbContext.Positions.Where(x => x.ParentComponent.ComponentId == p.ParentComponent.ComponentId).ToList();
            
            // if the new position is managerial, add it to the top of the Lineup and move all others down
            if (p.IsManager == true)
            {
                foreach (Position child in p.ParentComponent.Positions)
                {
                    // this shouldn't happen, but just in case the new position is manager and the parent component already has a managerial position 
                    child.IsManager = false;
                    if (child.PositionId != p.PositionId)
                    {
                        child.LineupPosition++;
                    }                        
                }
                // ensure the LineupPosition of a Managerial Position is set to 0
                p.LineupPosition = 0;
                ApplicationDbContext.Positions.Add(p);
            }
            if (p.LineupPosition == null)
            {
                // if this isn't set, we assume we are just adding the new Position to the end of the line
                int lastPositionQueuePosition = p.ParentComponent.Positions.Count();
                p.LineupPosition = lastPositionQueuePosition;
                ApplicationDbContext.Positions.Add(p);
            }
            else
            {
                if (p.PositionId == 0)
                {
                    // if the Position's LineupPosition is set, we need to update all of the existing Position's lineup positions to reflect this.
                    // we need all Positions in the ParentComponent whose .LineupPositions are >= the new Position's Lineup Position
                    List<Position> positionsToAdjust = p.ParentComponent.Positions.Where(x => x.LineupPosition >= p.LineupPosition).ToList();
                    foreach (Position child in positionsToAdjust)
                    {   
                        child.LineupPosition++;
                    }
                    ApplicationDbContext.Positions.Add(p);
                }
                else
                {                    
                    int? oldLineupIndex = ApplicationDbContext.Positions.FirstOrDefault(x => x.PositionId == p.PositionId).LineupPosition;
                    if (oldLineupIndex > p.LineupPosition) // position has been moved "up" in the lineup
                    {
                        List<Position> positionsToAdjust = p.ParentComponent.Positions.Where(x => x.LineupPosition >= p.LineupPosition && x.LineupPosition < oldLineupIndex).ToList();
                        foreach (Position child in positionsToAdjust)
                        {
                            if (child.PositionId != p.PositionId)
                            {
                                child.LineupPosition++;
                            }
                        }
                    }
                    else if (oldLineupIndex < p.LineupPosition) // position has been moved "down" the lineup
                    {
                        List<Position> positionsToAdjust = p.ParentComponent.Positions.Where(x => x.LineupPosition <= p.LineupPosition && x.LineupPosition > oldLineupIndex).ToList();
                        foreach (Position child in positionsToAdjust)
                        {
                            if (child.PositionId != p.PositionId)
                            {
                                child.LineupPosition--;
                            }
                        }
                    }
                    Position positionToUpdate = ApplicationDbContext.Positions.FirstOrDefault(x => x.PositionId == p.PositionId);
                    positionToUpdate.IsManager = p.IsManager;
                    positionToUpdate.IsUnique = p.IsUnique;
                    positionToUpdate.JobTitle = p.JobTitle;
                    positionToUpdate.LineupPosition = p.LineupPosition;
                    positionToUpdate.Name = p.Name;
                    positionToUpdate.ParentComponent = p.ParentComponent;                    
                }
            }
        }
    }
}
