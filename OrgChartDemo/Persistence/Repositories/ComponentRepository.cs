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
        /// Gets the list of <see cref="T:OrgChartDemo.Models.ChartableComponentWithMember"/>s.
        /// </summary>
        /// <returns>A <see cref="T:IEnumerable{T}"/> list of <see cref="T:OrgChartDemo.Models.ChartableComponentWithMember"/> objects</returns>
        public IEnumerable<ChartableComponentWithMember> GetOrgChartComponentsWithMembers()
        {
            int dynamicUniqueId = 10000; // don't ask... I need (id) fields that I can assign to (n) dynamic Chartables, and I need to ensure they will be unique and won't collide with the Component.ComponentId            
            List<ChartableComponentWithMember> results = new List<ChartableComponentWithMember>();
            foreach (Component c in GetAll())
            {
                foreach (Position p in c.Positions)
                {
                    if (p.IsManager) // EVERY COMPONENT MUST HAVE A POSITION THAT IS MANAGER
                    {
                        // if the position is Manager, there should only be one Member in the p.Members collection
                        // A component's Manager Position and Member info is rendered into the Chart Node with the Component
                        Member m = p.Members.FirstOrDefault(); // EVERY MANAGER POSITION MUST HAVE A MEMBER
                        ChartableComponentWithMember n = new ChartableComponentWithMember
                        {
                            Id = c.ComponentId,
                            Parentid = c?.ParentComponent?.ComponentId,
                            ComponentName = c.Name,
                            PositionId = p.PositionId,
                            PositionName = p.Name,
                            MemberId = m.MemberId,
                            MemberName = m.GetTitleName(),
                            Email = m.Email
                        };
                        results.Add(n);
                    }
                    else if (p.Members.Count > 0)
                    {
                        // we have a non-manager, non-unique position (these positions will contain most Members)
                        foreach (Member m in p.Members)
                        {
                            ChartableComponentWithMember n = new ChartableComponentWithMember
                            {
                                Id = dynamicUniqueId,
                                Parentid = c.ComponentId,
                                ComponentName = c.Name,
                                PositionId = p.PositionId,
                                PositionName = p.Name,
                                MemberId = m.MemberId,
                                MemberName = m.GetTitleName(),
                                Email = m.Email
                            };
                            results.Add(n);
                            dynamicUniqueId--;
                        }
                    }
                }
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
    }
}
