using BlueDeck.Models.APIModels;
using BlueDeck.Models.Types;
using BlueDeck.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.Repositories
{
    /// <summary>
    /// An implementation of <see cref="T:BlueDeck.Models.Repositories.IRepository{T}"/>
    /// </summary>
    /// <seealso cref="T:BlueDeck.Models.Repositories.IRepository{BlueDeck.Models.Component}" />
    public interface IComponentRepository : IRepository<Component>
    {
        /// <summary>
        /// Gets the list of <see cref="T:BlueDeck.Models.ChartableComponent"/>s.
        /// </summary>
        /// <remarks>
        /// This method is used to seed the GetOrgChart JQuery chart with a list of Components without Member details .
        /// </remarks>
        /// <returns>A <see cref="T:IEnumerable{T}"/> list of <see cref="T:BlueDeck.Models.ChartableComponent"/> objects</returns>
        IEnumerable<ChartableComponent> GetOrgChartComponentsWithoutMembers();

        /// <summary>
        /// Gets the list of <see cref="T:BlueDeck.Models.ChartableComponentWithMember"/>s.
        /// </summary>
        /// <remarks>
        /// This method is used to seed the GetOrgChart JQuery chart with a list of Components with Member details.
        /// </remarks>
        /// <returns>A <see cref="T:IEnumerable{T}"/> list of <see cref="T:BlueDeck.Models.ChartableComponentWithMember"/> objects</returns>
        List<ChartableComponentWithMember> GetOrgChartComponentsWithMembers(int parentComponentId);
        List<ChartableComponentWithMember> GetOrgChartComponentsWithMembersNoMarkup(int parentComponentId);
        List<Component> GetComponentAndChildren(int parentComponentId, List<Component> ccl);
        List<Component> GetComponentsAndChildrenSP(int parentComponentId);
        List<Component> GetComponentsAndChildrenWithParentSP(int parentComponentId);

        /// <summary>
        /// Gets the list of <see cref="T:BlueDeck.Models.Types.ComponentSelectListItem"/>s to populate a Component select list
        /// </summary>
        /// <returns>A <see cref="T:List{BlueDeck.Models.Types.ComponentSelectListItem}"/></returns>
        List<ComponentSelectListItem> GetComponentSelectListItems();

        /// <summary>
        /// Gets the component with all of it's member children.
        /// </summary>
        /// <param name="id">The Component identifier.</param>
        /// <returns>A <see cref="T:BlueDeck.Models.Component"/></returns>
        Component GetComponentWithChildren(int id);
        /// <summary>
        /// Gets the component with it's Parent Component loaded
        /// </summary>
        /// <param name="id">The ComponentId of the desired Component Entity.</param>
        /// <returns></returns>
        Component GetComponentWithParentComponent(int id);
        /// <summary>
        /// Gets the Component with it's Positions.
        /// </summary>
        /// <param name="id">The ComponentId of the requested Component.</param>
        /// <returns></returns>
        Component GetComponentWithPositions(int id);
        /// <summary>
        /// Gets the list components with all member children.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.IEnumerable{BlueDeck.Models.Component}"/></returns>
        IEnumerable<Component> GetComponentsWithChildren();

        List<PositionLineupItem> GetPositionLineupItemsForComponent(int componentId);

        List<ComponentPositionLineupItem> GetComponentLineupItemsForComponent(int componentId);
        void UpdateComponentAndSetLineup(Component c);
        void RemoveComponent(int componentId);

        /// <summary>
        /// Gets the number of child Components of a given ComponentId.
        /// </summary>
        /// <remarks>
        /// This method can be used to determine if a Component has children. It is mainly used to prevent deletion of 
        /// Components with active children assigned to it.
        /// </remarks>
        /// <param name="componentId">The component identifier.</param>
        /// <returns></returns>
        int GetChildComponentCountForComponent(int componentId);

        /// <summary>
        /// Determines if a Component Name is available.
        /// </summary>
        /// <remarks>
        /// This method will determine if a Component Name is available.
        /// It requires a <see cref="Component"/> parameter because the method needs to validate
        /// that a Component name isn't colliding with itself.
        /// </remarks>
        /// <param name="c">The Component to to check.</param>
        /// <returns></returns>
        bool ComponentNameNotAvailable(Component c);
        List<ComponentSelectListItem> GetChildComponentsForComponentId(int componentId);
        List<Member> GetMembersRosterForComponentId(int componentId);
        AdminComponentIndexListViewModel GetAdminComponentIndexListViewModel();
        Task<ComponentApiResult> GetApiComponent(int id);
    }
}
