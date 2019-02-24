using Microsoft.EntityFrameworkCore;
using OrgChartDemo.Models;
using OrgChartDemo.Models.Repositories;
using OrgChartDemo.Models.Types;
using System.Collections.Generic;
using System.Linq;


namespace OrgChartDemo.Persistence.Repositories
{
    /// <summary>
    /// A repository for the Component Entity
    /// </summary>
    /// <seealso cref="T:OrgChartDemo.Persistence.Repositories.Repository{OrgChartDemo.Models.Component}" />
    /// <seealso cref="T:OrgChartDemo.Models.Repositories.IComponentRepository" />
    public class ComponentRepository : Repository<Component>, IComponentRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:OrgChartDemo.Persistence.Repositories.ComponentRepository"/> class.
        /// </summary>
        /// <param name="context">The <see cref="T:OrgChartDemo.Models.ApplicationDbContext"/></param>
        public ComponentRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public ApplicationDbContext ApplicationDbContext {
            get { return Context as ApplicationDbContext; }
        }
        /// <summary>
        /// Gets the list components with all member children.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.IEnumerable{OrgChartDemo.Models.Component}" />
        /// </returns>
        public IEnumerable<Component> GetComponentsWithChildren()
        {
            return ApplicationDbContext.Components.Include(x => x.Positions)
                .ThenInclude(x => x.Members)
                .ThenInclude(x => x.Rank)
                .Where(x => x.ParentComponent != null)
                .ToList(); 
        }

        /// <summary>
        /// Gets the component with all of it's member children.
        /// </summary>
        /// <param name="id">The Component identifier.</param>
        /// <returns>
        /// A <see cref="T:OrgChartDemo.Models.Component" />
        /// </returns>
        public Component GetComponentWithChildren(int id)
        {
            return ApplicationDbContext.Components
                .Where(x => x.ComponentId == id)
                .Include(x => x.Positions).ThenInclude(x => x.Members).ThenInclude(x => x.Rank)
                .Include(x => x.Positions).ThenInclude(x => x.Members).ThenInclude(x => x.Gender)
                .Include(x => x.Positions).ThenInclude(x => x.Members).ThenInclude(x => x.Race)
                .Include(x => x.Positions).ThenInclude(x => x.Members).ThenInclude(x => x.DutyStatus)                                
                .FirstOrDefault();
            
        }

        /// <summary>
        /// Gets the Component with it's Positions.
        /// </summary>
        /// <param name="id">The ComponentId of the requested Component.</param>
        /// <returns></returns>
        public Component GetComponentWithPositions(int id)
        {
            return ApplicationDbContext.Components
                .Where(x => x.ComponentId == id)
                .Include(x => x.Positions)
                .FirstOrDefault();
        }
               

        /// <summary>
        /// Gets the list of <see cref="T:OrgChartDemo.Models.ChartableComponent"/>s.
        /// </summary>
        /// <returns>A <see cref="T:IEnumerable{T}"/> list of <see cref="T:OrgChartDemo.Models.ChartableComponent"/> objects</returns>
        public IEnumerable<ChartableComponent> GetOrgChartComponentsWithoutMembers()
        {
            List<ChartableComponent> results = new List<ChartableComponent>();
            foreach (Component c in GetAll())
            {
                ChartableComponent n = new ChartableComponent
                {
                    Id = c.ComponentId,
                    Parentid = c?.ParentComponent?.ComponentId,
                    ComponentName = c.Name
                };
                results.Add(n);
            }
            return results;
        }       

        /// <summary>
        /// Gets the list of <see cref="T:OrgChartDemo.Models.Types.ComponentSelectListItem" />s to populate a Component select list
        /// </summary>
        /// <returns>
        /// A <see cref="T:List{OrgChartDemo.Models.Types.ComponentSelectListItem}" />
        /// </returns>
        public List<ComponentSelectListItem> GetComponentSelectListItems()
        {
            return GetAll().ToList().ConvertAll(x => new ComponentSelectListItem { ComponentName = x.Name, Id = x.ComponentId });
        }

        public List<Component> GetComponentAndChildren(int parentComponentId, List<Component> ccl){
            //List<Component> children = Find(x => x.ParentComponent.ComponentId == parentComponentId).ToList();

            // This query ONLY determines if we have reached the bottom of the depth chart
            // there are no includes because we retrieve the full component details when we cascade back up the recursion chain            
            List<Component> children = ApplicationDbContext.Components
                .Where(x => x.ParentComponent.ComponentId == parentComponentId).ToList();
            if (children.Count() != 0){
                foreach(Component c in children){
                    GetComponentAndChildren(c.ComponentId, ccl);     
                } 
            }
            Component parent = ApplicationDbContext.Components
                .Where(x => x.ComponentId == parentComponentId)
                .Include(x => x.ParentComponent)                
                .Include(x => x.Positions).ThenInclude(y => y.Members).ThenInclude(z => z.Rank)
                .Include(x => x.Positions).ThenInclude(y => y.Members).ThenInclude(z => z.Gender)
                .Include(x => x.Positions).ThenInclude(y => y.Members).ThenInclude(x => x.Race)
                .Include(x => x.Positions).ThenInclude(y => y.Members).ThenInclude(x => x.DutyStatus)                
                .FirstOrDefault();
            if (parent != null){
                ccl.Add(parent);    
            }            
            return ccl;
        }        
        
        /// <summary>
        /// Gets the list of <see cref="T:OrgChartDemo.Models.ChartableComponentWithMember"/>s.
        /// </summary>
        /// <returns>A <see cref="T:IEnumerable{T}"/> list of <see cref="T:OrgChartDemo.Models.ChartableComponentWithMember"/> objects</returns>
        public IEnumerable<ChartableComponentWithMember> GetOrgChartComponentsWithMembers()
        {
            int dynamicUniqueId = 10000; // don't ask... I need (id) fields that I can assign to (n) dynamic Chartables, and I need to ensure they will be unique and won't collide with the Component.ComponentId  
            List<ChartableComponentWithMember> results = new List<ChartableComponentWithMember>();
            IEnumerable<Component> TestList = GetComponentsWithChildren();
            foreach (Component c in GetComponentsWithChildren())
            {
                // All components will render this at minimum
                ChartableComponentWithMember n = new ChartableComponentWithMember  {
                    Id = c.ComponentId,
                    Parentid = c?.ParentComponent?.ComponentId,
                    ComponentName = c.Name
                    };  
                // Check if component has child positions
                if (c.Positions.Count > 0)
                {
                    // has child positions, so we need chartables for all
                    foreach (Position p in c.Positions)
                    {
                        // first, check if Position is Manager. If so, we want to render member details in the Parent Component Node
                        if (p.IsManager)
                        {
                            // if no member is assigned to a Position designated as Manager, then we want to render "Vacant" details in the Parent Node
                            if (p.Members.Count == 0)
                            {
                                n.PositionName = p.Name;                                
                                n.MemberName = "Vacant";
                                n.MemberId = -1;
                                n.Email = "<a href='mailto:Admin@BlueDeck.com'>Mail the Admin</a>";
                            }
                            else
                            {
                                n.PositionName = p.Name;
                                n.PositionId = p.PositionId;
                                n.MemberName = p.Members.First().GetTitleName();
                                n.Email = $"<a href='mailto:{p.Members.First().Email}'>{p.Members.First().Email}</a>";
                                n.MemberId = p.Members.First().MemberId;
                            }
                        }
                        else if (p.IsUnique)
                        {
                            dynamicUniqueId--;
                            ChartableComponentWithMember d = new ChartableComponentWithMember
                            {
                                Id = dynamicUniqueId,
                                Parentid = n.Id,
                                ComponentName = p.Name,                                
                            };
                            if (p.Members.Count == 0)
                            {
                                d.PositionId = p.PositionId;
                                d.MemberName = "Vacant";
                                d.MemberId = -1;
                                d.Email = "Admin@BlueDeck.com";
                            }
                            else
                            {
                                d.PositionId = p.PositionId;
                                d.MemberName = p.Members.First().GetTitleName();
                                d.Email = p.Members.First().Email;
                                d.MemberId = p.Members.First().MemberId;
                            }
                            results.Add(d);
                        }
                        else if (p.Members.Count() > 0 ) 
                            // if position is not manager/unique and has members, we need a new Chartable for each member
                        {
                            foreach (Member m in p.Members)
                            {
                                dynamicUniqueId--;
                                ChartableComponentWithMember x = new ChartableComponentWithMember {
                                    Id = dynamicUniqueId,
                                    Parentid = n.Id,
                                    ComponentName = p.Name, // TODO: Change this to "Node Name" in GetOrgChart?
                                    MemberName = m.GetTitleName(),
                                    Email = m.Email,
                                    MemberId = m.MemberId,
                                    PositionId = p.PositionId
                                    };                                    
                                results.Add(x);
                            }
                        }
                    }
                }
                results.Add(n);
            }
            return results;
        }        

        public List<PositionLineupItem> GetPositionLineupItemsForComponent(int componentId)
        {
            return ApplicationDbContext.Positions
                .Where(x => x.ParentComponent.ComponentId == componentId) 
                .OrderByDescending(x => x.LineupPosition)
                .ToList()
                .ConvertAll(x => new PositionLineupItem(x));
        }
       
        public List<ComponentPositionLineupItem> GetComponentLineupItemsForComponent(int componentId)
        {
            return ApplicationDbContext.Components
                .Where(x => x.ParentComponent.ComponentId == componentId)
                .OrderBy(x => x.LineupPosition)
                .ToList()
                .ConvertAll(x => new ComponentPositionLineupItem(x));
        }

        public void UpdateComponentAndSetLineup(Component c)
        {
            // assume that the Position's ParentComponent is set? It has to be, right? Hmmm...
            // I don't assume that the Position's ParentComponent's Child Positions collection will be set...
            List<Component> siblings = ApplicationDbContext.Components.Where(x => x.ParentComponent.ComponentId == x.ParentComponent.ComponentId).ToList();

            // if the new position is managerial, add it to the top of the Lineup and move all others down
            foreach (Position sibling in siblings)
            {
                // this shouldn't happen, but just in case the new position is manager and the parent component already has a managerial position                 
                if (sibling.PositionId != p.PositionId)
                {
                    sibling.LineupPosition++;
                }
            }
                // ensure the LineupPosition of a Managerial Position is set to 0
                p.LineupPosition = 0;
                ApplicationDbContext.Positions.Add(p);
            
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
