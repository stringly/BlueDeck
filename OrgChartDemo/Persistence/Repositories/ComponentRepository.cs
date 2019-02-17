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
       
    }
}
