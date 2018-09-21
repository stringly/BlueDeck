using OrgChartDemo.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace OrgChartDemo.Models {
    /// <summary>
    /// IComponentRepository derivative used to create Entity Framework context-based repositories for dependency injection.
    /// </summary>
    /// <seealso cref="OrgChartDemo.Models.IComponentRepository" />
    public class EFComponentRepository : IComponentRepository {
        private ApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="EFComponentRepository"/> class.
        /// </summary>
        /// <param name="ctx">The an instance of <see cref="ApplicationDbContext"/></param>
        public EFComponentRepository(ApplicationDbContext ctx) {
            context = ctx;
        }

        /// <summary>
        /// Gets the list of <see cref="Position"/>s.
        /// </summary>
        /// <value>
        /// The <see cref="IEnumerable{T}"/> list of <see cref="Position"/>s in the repository.
        /// </value>
        public List<Position> Positions => context.Positions.Include(x => x.ParentComponent).ToList();

        /// <summary>
        /// Gets the list of <see cref="Component"/>s.
        /// </summary>
        /// <value>
        /// The <see cref="IEnumerable{T}"/> list of <see cref="Component"/>s in the repository.
        /// </value>
        public List<Component> Components => context.Components.Include(x => x.Positions).ToList();

        /// <summary>
        /// Gets the list of <see cref="Member"/>s.
        /// </summary>
        /// <value>
        /// The <see cref="IEnumerable{T}"/> list of <see cref="Member"/>s in the repository.
        /// </value>
        public List<Member> Members => context.Members.ToList();

        /// <summary>
        /// Gets the list of <see cref="ChartableComponent"/>s.
        /// </summary>
        /// <returns>A <see cref="IEnumerable{T}"/> list of <see cref="ChartableComponent"/> objects</returns>
        public IEnumerable<ChartableComponent> GetOrgChartComponentsWithoutMembers()
        {
            List<ChartableComponent> results = new List<ChartableComponent>();
            foreach (Component c in Components)
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
            foreach (Component c in Components)
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

        /// <summary>
        /// Gets the list of <see cref="PositionWithMemberCountItem"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> list of <see cref="PositionWithMemberCountItem"/>s/></returns>
        public IEnumerable<PositionWithMemberCountItem> GetPositionListWithMemberCount()
        {
            var results = new List<PositionWithMemberCountItem>();
            
            foreach (Position p in Positions)
            {                
                results.Add(new PositionWithMemberCountItem(p));
            }
            return results;
        }

        /// <summary>
        /// Adds a <see cref="Position"/> to the Positions collection.
        /// </summary>
        /// <remarks>
        /// This depends on <see cref="OrgChartDemo.Models.ExtensionMethods.ExtensionMethods.Add{T}(IEnumerable{T}, T)"/> to add an item to an <see cref="IEnumerable{T}"/>
        /// </remarks>
        /// <param name="p">A <see cref="Position"/> to add to the Positions collection.</param>
        public void AddPosition(Position p)
        {
            context.Positions.Add(p);
            context.SaveChanges();
        }

        /// <summary>
        /// Removes the position.
        /// </summary>
        /// <param name="PositionIdToRemove">The position identifier to remove.</param>
        public void RemovePosition(int PositionIdToRemove)
        {            
            context.Positions.Remove(Positions.SingleOrDefault(x => x.PositionId == PositionIdToRemove));            
            context.SaveChanges();
        }

        /// <summary>
        /// Edits the position.
        /// </summary>
        /// <param name="p">The <see cref="Position"/> to edit</param>
        public void EditPosition(Position p)
        {
            Position old = context.Positions.SingleOrDefault(x => x.PositionId == p.PositionId);
            if (old != null)
            {
                old.ParentComponent = p.ParentComponent;
                old.Name = p.Name;
                old.JobTitle = p.JobTitle;
                old.IsUnique = p.IsUnique;
                old.IsManager = p.IsManager;
                context.SaveChanges();
            }            
        }
    }
}
