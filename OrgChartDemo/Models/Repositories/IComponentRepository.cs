using OrgChartDemo.Models.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models.Repositories
{
    /// <summary>
    /// An implementation of <see cref="T:OrgChartDemo.Models.Repositories.IRepository{T}"/>
    /// </summary>
    /// <seealso cref="T:OrgChartDemo.Models.Repositories.IRepository{OrgChartDemo.Models.Component}" />
    public interface IComponentRepository : IRepository<Component>
    {
        /// <summary>
        /// Gets the list of <see cref="T:OrgChartDemo.Models.ChartableComponent"/>s.
        /// </summary>
        /// <remarks>
        /// This method is used to seed the GetOrgChart JQuery chart with a list of Components without Member details .
        /// </remarks>
        /// <returns>A <see cref="T:IEnumerable{T}"/> list of <see cref="T:OrgChartDemo.Models.ChartableComponent"/> objects</returns>
        IEnumerable<ChartableComponent> GetOrgChartComponentsWithoutMembers();

        /// <summary>
        /// Gets the list of <see cref="T:OrgChartDemo.Models.ChartableComponentWithMember"/>s.
        /// </summary>
        /// <remarks>
        /// This method is used to seed the GetOrgChart JQuery chart with a list of Components with Member details.
        /// </remarks>
        /// <returns>A <see cref="T:IEnumerable{T}"/> list of <see cref="T:OrgChartDemo.Models.ChartableComponentWithMember"/> objects</returns>
        IEnumerable<ChartableComponentWithMember> GetOrgChartComponentsWithMembers();

        List<Component> GetComponentAndChildren(int parentComponentId, List<Component> ccl);

        /// <summary>
        /// Gets the list of <see cref="T:OrgChartDemo.Models.Types.ComponentSelectListItem"/>s to populate a Component select list
        /// </summary>
        /// <returns>A <see cref="T:List{OrgChartDemo.Models.Types.ComponentSelectListItem}"/></returns>
        List<ComponentSelectListItem> GetComponentSelectListItems();

        /// <summary>
        /// Gets the component with all of it's member children.
        /// </summary>
        /// <param name="id">The Component identifier.</param>
        /// <returns>A <see cref="T:OrgChartDemo.Models.Component"/></returns>
        Component GetComponentWithChildren(int id);

        /// <summary>
        /// Gets the Component with it's Positions.
        /// </summary>
        /// <param name="id">The ComponentId of the requested Component.</param>
        /// <returns></returns>
        Component GetComponentWithPositions(int id);
        /// <summary>
        /// Gets the list components with all member children.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.IEnumerable{OrgChartDemo.Models.Component}"/></returns>
        IEnumerable<Component> GetComponentsWithChildren();

        List<PositionLineupItem> GetPositionLineupItemsForComponent(int componentId);
    }
}
