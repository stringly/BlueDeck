using OrgChartDemo.Models;
using OrgChartDemo.Models.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace OrgChartDemo.Persistence.Repositories
{
    public class ComponentRepository : Repository<Component>, IComponentRepository
    {
        public ComponentRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Gets the list of <see cref="ChartableComponent"/>s.
        /// </summary>
        /// <returns>A <see cref="IEnumerable{T}"/> list of <see cref="ChartableComponent"/> objects</returns>
        public IEnumerable<ChartableComponent> GetOrgChartComponentsWithoutMembers()
        {
            List<ChartableComponent> results = new List<ChartableComponent>();
            foreach (Component c in GetAll())
            {
                ChartableComponent n = new ChartableComponent
                {
                    id = c.ComponentId,
                    parentid = c?.ParentComponent?.ComponentId,
                    componentName = c.Name
                };
                results.Add(n);
            }
            return results;
        }

        /// <summary>
        /// Gets the list of <see cref="ChartableComponentWithMember"/>s.
        /// </summary>
        /// <returns>A <see cref="IEnumerable{T}"/> list of <see cref="ChartableComponentWithMember"/> objects</returns>
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
                            id = c.ComponentId,
                            parentid = c?.ParentComponent?.ComponentId,
                            componentName = c.Name,
                            positionId = p.PositionId,
                            positionName = p.Name,
                            memberId = m.MemberId,
                            memberName = m.GetTitleName(),
                            email = m.Email
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
                                id = dynamicUniqueId,
                                parentid = c.ComponentId,
                                componentName = c.Name,
                                positionId = p.PositionId,
                                positionName = p.Name,
                                memberId = m.MemberId,
                                memberName = m.GetTitleName(),
                                email = m.Email
                            };
                            results.Add(n);
                            dynamicUniqueId--;
                        }
                    }
                }
            }
            return results;
        }

    }
}
