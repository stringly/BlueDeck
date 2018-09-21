using System.Collections.Generic;
using System.Linq;
using OrgChartDemo.Models.ViewModels;
using OrgChartDemo.Models.ExtensionMethods;
using Microsoft.EntityFrameworkCore;


namespace OrgChartDemo.Models {
    /// <summary>
    /// IComponentRepository derivative used to create repositories for testing.
    /// </summary>
    /// <seealso cref="OrgChartDemo.Models.IComponentRepository" />
    public class FakeRepository : IComponentRepository {

        /// <summary>
        /// Gets or sets the collection of Position Entites.
        /// </summary>
        /// <value>
        /// The Position Entities collection.
        /// </value>
        public virtual List<Position> Positions { get; set; }

        /// <summary>
        /// Gets or sets the collection of Component Entities.
        /// </summary>
        /// <value>
        /// The Component Entities collection.
        /// </value>
        public virtual List<Component> Components { get; set; }

        /// <summary>
        /// Gets or sets the collection of Member Entities.
        /// </summary>
        /// <value>
        /// The Member Entities collection
        /// </value>
        public virtual List<Member> Members { get; set; }

        /// <summary>
        /// Gets a list of ChartableComponents without Member info.
        /// </summary>
        /// <returns>A List of ChartableComponent types</returns>
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
        /// Gets a list of ChartableComponents Member info.
        /// </summary>
        /// <returns>A List of ChartableComponentWithMember types</returns>
        public IEnumerable<ChartableComponentWithMember> GetOrgChartComponentsWithMembers() {
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
            Positions.Add(p);
        }

        /// <summary>
        /// Removes the Position with the provieded Id.
        /// </summary>
        /// <param name="PositionIdToRemove">The PositionId of the Position to remove.</param>
        public void RemovePosition(int PositionIdToRemove)
        {            
            Positions.Remove(Positions.Where(x => x.PositionId == PositionIdToRemove).FirstOrDefault());            
        }

        /// <summary>
        /// Edits the position.
        /// </summary>
        /// <param name="p">The <see cref="Position"/> to edit/update</param>
        public void EditPosition(Position p)
        {
            Position old = Positions.Find(x => x.PositionId == p.PositionId);
            old.ParentComponent = p.ParentComponent;
            old.Name = p.Name;
            old.JobTitle = p.JobTitle;
            old.IsUnique = p.IsUnique;
            old.IsManager = p.IsManager;
        }

    }
}