using Microsoft.EntityFrameworkCore;
using OrgChartDemo.Models;
using OrgChartDemo.Models.Repositories;
using OrgChartDemo.Models.Types;
using System.Collections.Generic;
using System.Data.SqlClient;
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
            Component result = ApplicationDbContext.Components
                .Where(x => x.ComponentId == id)
                .Include(x => x.Positions).ThenInclude(x => x.Members).ThenInclude(x => x.Rank)
                .Include(x => x.Positions).ThenInclude(x => x.Members).ThenInclude(x => x.Gender)
                .Include(x => x.Positions).ThenInclude(x => x.Members).ThenInclude(x => x.Race)
                .Include(x => x.Positions).ThenInclude(x => x.Members).ThenInclude(x => x.DutyStatus)                                
                .FirstOrDefault();
            if (result != null)
            {
                result.ChildComponents = new List<Component>();
                result.ChildComponents = ApplicationDbContext.Components.Where(x => x.ParentComponent.ComponentId == result.ComponentId).ToList();
            }
            return result;
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
        
        public List<Component> GetComponentsAndChildrenSP(int parentComponentId)
        {
            SqlParameter param1 = new SqlParameter("@ComponentId", parentComponentId);
            
            List<Component> components = ApplicationDbContext.Components.FromSql("dbo.GetComponentAndChildrenDemo @ComponentId", param1).ToList();
            ApplicationDbContext.Set<Position>().Where(x => components.Contains(x.ParentComponent))
                .Include(y => y.Members).ThenInclude(z => z.Rank)
                .Include(y => y.Members).ThenInclude(z => z.Gender)
                .Include(y => y.Members).ThenInclude(x => x.Race)
                .Include(y => y.Members).ThenInclude(x => x.DutyStatus) 
                .Load();

            return components;
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

        /// <summary>
        /// Adds a new Component or Updates an existing Component and adjusts the LineupPosition of all sibling Components within the ParentComponent
        /// </summary>
        /// <remarks>
        /// If this method is used to edit an existing component, then the Component parameter must be a new Component instance set to the form values of the existing component.
        /// This is because the method needs to know the "unedited" value of the Component's LineupPosition to determine whether the Component has been moved up or down
        /// in it's ParentComponent's Lineup.
        /// /// </remarks>
        /// <param name="c">The <seealso cref="T:OrgChartDemo.Models.Component"/> being added or edited.</param>
        public void UpdateComponentAndSetLineup(Component c)
        {
            // assume that the Component's ParentComponent is set? It has to be, right? Hmmm...
        

            /* What needs to happen here: 
            *  If the c parameter is a new Component: get the LineupPosition of the new Component, and then adjust all of the current siblings based on the new lineup
            */

            if (c.ComponentId == 0) { // ComponentId = 0 is a new Component
                // get a list of only sibling Components that will be "pushed up" the Lineup because we are adding a new position
                List<Component> siblings = ApplicationDbContext.Components
                    .Where(x => x.ParentComponent.ComponentId == c.ParentComponent.ComponentId && x.LineupPosition >= c.LineupPosition)
                    .ToList();
                foreach (Component sibling in siblings)
                {                
                    sibling.LineupPosition++;  
                }
                // add the new Component to the Context
                ApplicationDbContext.Components.Add(c);
            }
            else // Component parameter is an existing component
            {
                // retrieve the old LineupPosition of the Component being edited to determine if the Component has moved up or down the lineup list
                int? oldLineupPosition = ApplicationDbContext.Components.FirstOrDefault(x => x.ComponentId == c.ComponentId).LineupPosition;

                if (oldLineupPosition > c.LineupPosition) // Component has been moved "up" the list
                {
                    // retrieve the sibling list... remember, this will include the Component being edited, so it needs to be skipped
                    List<Component> siblingsToAdjust = ApplicationDbContext.Components.Where(x => x.ParentComponent.ComponentId == c.ParentComponent.ComponentId && x.LineupPosition < oldLineupPosition).ToList();
                    foreach(Component sibling in siblingsToAdjust)
                    {
                        if (sibling.ComponentId != c.ComponentId) // skip if the sibling is the Component being edited
                        {
                            sibling.LineupPosition++;
                        }
                    }
                }
                else if (oldLineupPosition < c.LineupPosition) // Component has been moved "down" the list
                {
                    // retrieve the sibling list... remember, this will include the Component being edited, so it needs to be skipped
                    List<Component> siblingsToAdjust = ApplicationDbContext.Components.Where(x => x.ParentComponent.ComponentId == c.ParentComponent.ComponentId && x.LineupPosition > oldLineupPosition).ToList();

                    foreach(Component sibling in siblingsToAdjust)
                    {
                        if (sibling.ComponentId != c.ComponentId)
                        {
                            sibling.LineupPosition--;
                        }
                    }
                }

                // finally, update the Component being edited
                Component componentBeingEdited = ApplicationDbContext.Components.FirstOrDefault(x => x.ComponentId == c.ComponentId);
                componentBeingEdited.Acronym = c.Acronym;
                componentBeingEdited.LineupPosition = c.LineupPosition;
                componentBeingEdited.Name = c.Name;
                componentBeingEdited.ParentComponent = c.ParentComponent;
            }
        }

        public void RemoveComponent(int componentId)
        {
            // Prevent deleting Components with Children Components
            if (ApplicationDbContext.Components.Where(x => x.ParentComponent.ComponentId == componentId) != null)
            {
                return;
            }
            else
            {
                Component toRemove = ApplicationDbContext.Components.SingleOrDefault(x => x.ComponentId == componentId);

                if (toRemove != null)
                {
                    List<Position> cPositions = ApplicationDbContext.Positions
                        .Where(x => x.ParentComponent.ComponentId == componentId)
                        .Include(x => x.Members)
                        .ToList();
                    Position unassigned = ApplicationDbContext.Positions
                        .Where(x => x.Name == "Unassigned").SingleOrDefault();
                    foreach (Position p in cPositions)
                    {
                        foreach (Member m in p.Members)
                        {
                            m.Position = unassigned;

                        }

                    }
                    ApplicationDbContext.SaveChanges();
                    ApplicationDbContext.Positions.RemoveRange(cPositions);
                    ApplicationDbContext.Components.Remove(toRemove);
                }
            }                            
        }
    }
}
