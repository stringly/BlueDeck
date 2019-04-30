using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BlueDeck.Models;
using BlueDeck.Models.Repositories;
using BlueDeck.Models.Types;
using BlueDeck.Models.ViewModels;
using BlueDeck.Models.APIModels;
using System.Threading.Tasks;

namespace BlueDeck.Persistence.Repositories
{
    /// <summary>
    /// A repository for the Position entity
    /// </summary>
    /// <seealso cref="T:BlueDeck.Persistence.Repositories.Repository{BlueDeck.Models.Position}" />
    /// <seealso cref="T:BlueDeck.Models.Repositories.IPositionRepository" />
    public class PositionRepository : Repository<Position>, IPositionRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:BlueDeck.Persistence.Repositories.PositionRepository"/> class.
        /// </summary>
        /// <param name="context">An <see cref="T:BlueDeck.Models.ApplicationDbContext"/>.</param>
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

        public List<PositionSelectListItem> GetAllPositionSelectListItems(){
            return GetAll().ToList().ConvertAll(x => new PositionSelectListItem(x));
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
            return ApplicationDbContext.Positions
                .Include(x => x.ParentComponent)
                .Where(x => x.PositionId == positionId)
                .Include(x => x.Creator)
                .Include(x => x.LastModifiedBy)
                .Include(x => x.Members).ThenInclude(x => x.Rank)
                .Include(x => x.Members).ThenInclude(x => x.Race)
                .Include(x => x.Members).ThenInclude(x => x.Gender)
                .SingleOrDefault();
        }
        
        /// <summary>
        /// Removes the position and reassigns any members.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="newPositionName">New name of the position.</param>
        public void RemovePositionAndReassignMembers(int id, string newPositionName = "Unassigned")
        {
            // first, reassign all members in the position to the General Pool
            Position toDelete = ApplicationDbContext.Positions
                .Include(x => x.ParentComponent)
                .Include(x => x.Members)
                .First(x => x.PositionId == id);
            Position toReassign = ApplicationDbContext.Positions.First(x => x.Name == newPositionName);
            toReassign.Members.AddRange(toDelete.Members);
            // adjust lineup for siblings here... if I am removing a Position, I only need to decrement the LineupPositions of all Positions "after" the
            // deleted Component's LineupPosition
            List<Position> siblings = ApplicationDbContext.Positions
                .Where(x => x.ParentComponent.ComponentId == toDelete.ParentComponent.ComponentId 
                && x.LineupPosition > toDelete.LineupPosition 
                && x.PositionId != toDelete.PositionId)
                .ToList();
            foreach(Position sibling in siblings)
            {
                sibling.LineupPosition--;
            }
            ApplicationDbContext.Positions.Remove(toDelete);
        }
        
        public Position GetPositionWithParentComponent(int positionId)
        {
            return ApplicationDbContext.Positions
                .Include(x => x.ParentComponent)
                .Include(x => x.Members)
                .Where(x => x.PositionId == positionId)
                .FirstOrDefault();
        }

        public void UpdatePositionAndSetLineup(Position p)
        {
            // assume that the Position's ParentComponent is set? It has to be, right? Hmmm...
            // I don't assume that the Position's ParentComponent's Child Positions collection will be set...
            p.ParentComponent.Positions = ApplicationDbContext.Positions.Where(x => x.ParentComponent.ComponentId == p.ParentComponent.ComponentId).ToList();
            
            if (p.PositionId == 0) // new Position
            {
                // if Manager, then push new Position to the top of the sibling list
                if (p.IsManager == true)
                {
                    foreach (Position child in p.ParentComponent.Positions)
                    {
                        // this shouldn't happen, but just in case the new position is manager and the parent component already has a managerial position 
                        child.IsManager = false;
                        child.LineupPosition++;
                        
                    }
                    // ensure the LineupPosition of a Managerial Position is set to 0
                    p.LineupPosition = 0;
                    
                }
                else if (p.LineupPosition == null)
                {
                    // if no LineupPosition set, then assume we add the new Position to the end of the sibling list
                    p.LineupPosition = p.ParentComponent.Positions.Count();
                }
                else // new Position has been assigned a specific Lineup Position, and others must be adjusted
                {
                    // We need to update all of the existing Position's lineup positions to reflect this.
                    // We need all Positions in the ParentComponent whose LineupPositions are >= the new Position's Lineup Position
                    List<Position> positionsToAdjust = p.ParentComponent.Positions.Where(x => x.LineupPosition >= p.LineupPosition).ToList();
                    foreach (Position child in positionsToAdjust)
                    {
                        child.LineupPosition++;
                    }
                }
                if(p.Members != null)
                {                    
                    foreach(Member m in p.Members)
                    {
                        Member repoMember = ApplicationDbContext.Members.Find(m.MemberId);
                        repoMember.Position = p;
                    }
                }
                ApplicationDbContext.Positions.Add(p);
            }
            else // existing Position
            {
                // if Manager, then push the Position to the top of the sibling list
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
                }
                else if (p.LineupPosition == null)
                {
                    // if this isn't set, we assume we are just adding the new Position to the end of the line
                    p.LineupPosition = p.ParentComponent.Positions.Count();
                }
                else // the Position has been assigned a specific Lineup Position, and others must be adjusted
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
                }
                // now, we check to see if the Position has been changed from Non-Unique to Unique and reassign it's current members if so
                
                // first, retrieve the existing Position from the repo
                Position positionToUpdate = ApplicationDbContext.Positions
                    .Include(x => x.Members)
                    .FirstOrDefault(x => x.PositionId == p.PositionId);
                if(p.IsUnique == true && positionToUpdate.Members.Count() > 1)
                {
                    int unassignedId = ApplicationDbContext.Positions.FirstOrDefault(x => x.Name == "Unassigned").PositionId;
                    foreach (Member m in positionToUpdate.Members)
                    {
                        m.PositionId = unassignedId;
                    }
                }
                if(p.Members != null)
                {
                    int unassignedId = ApplicationDbContext.Positions.FirstOrDefault(x => x.Name == "Unassigned").PositionId;
                    foreach (Member m in positionToUpdate.Members)
                    {
                        m.PositionId = unassignedId;
                    }
                    foreach (Member m in p.Members)
                    {
                        ApplicationDbContext.Members.Find(m.MemberId);
                        m.PositionId = p.PositionId;
                    }
                }

                // Finally, update the Position with the new values
                positionToUpdate.IsManager = p.IsManager;
                positionToUpdate.IsUnique = p.IsUnique;
                positionToUpdate.JobTitle = p.JobTitle;
                positionToUpdate.LineupPosition = p.LineupPosition;
                positionToUpdate.Name = p.Name;
                positionToUpdate.Callsign = p.Callsign;
                positionToUpdate.ParentComponent = p.ParentComponent;
                positionToUpdate.LastModified = p.LastModified;
                positionToUpdate.LastModifiedById = p.LastModifiedById;               

            }
        }

        public List<PositionSelectListItem> GetPositionsUserCanEdit(int componentId)
        {
            SqlParameter param1 = new SqlParameter("@ComponentId", componentId);
            return ApplicationDbContext.GetPositionsUserCanEdit.FromSql("EXECUTE Get_Positions_User_Can_Edit @ComponentId", param1).ToList();
        }

        public AdminPositionIndexListViewModel GetAdminPositionIndexListViewModel()
        {
            AdminPositionIndexListViewModel vm = new AdminPositionIndexListViewModel();
            vm.Positions = ApplicationDbContext.Positions
                .Include(x => x.ParentComponent)
                .Include(x => x.Members)
                .ToList()
                .ConvertAll(x => new AdminPositionIndexViewModelListItem(x));
            return vm;       
        }

        public async Task<PositionApiResult> GetApiPosition(int id)
        {
            Position position = await ApplicationDbContext.Positions
                .Include(x => x.Members).ThenInclude(x => x.Race)
                .Include(x => x.Members).ThenInclude(x => x.Gender)
                .Include(x => x.Members).ThenInclude(x => x.DutyStatus)
                .Include(x => x.Members).ThenInclude(x => x.Rank)
                .Include(x => x.Members).ThenInclude(x => x.PhoneNumbers).ThenInclude(x => x.Type)
                .FirstOrDefaultAsync(x => x.PositionId == id);

            return new PositionApiResult(position);
                    

        }
    }
}
