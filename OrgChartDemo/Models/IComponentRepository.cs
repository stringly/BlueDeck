using OrgChartDemo.Models.ViewModels;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models {
    /// <summary>
    /// Interface for OrgChartDemo Repository to facilitate dependency injection
    /// </summary>
    public interface IComponentRepository {

        /// <summary>
        /// Gets the list of <see cref="Position"/>s 
        /// </summary>
        /// <value>
        /// A <see cref="List{T}"/> of <see cref="Position"/>
        /// </value>
        List<Position> Positions { get; }

        /// <summary>
        /// Gets the list of <see cref="Component"/>s.
        /// </summary>
        /// <value>
        /// A <see cref="List{T}"/> of <see cref="Component"/>s
        /// </value>
        List<Component> Components { get; }

        /// <summary>
        /// Gets the list of <see cref="Member"/>s.
        /// </summary>
        /// <value>
        /// A <see cref="List{T}"/> of <see cref="Member"/>s
        /// </value>
        List<Member> Members { get; }

        /// <summary>
        /// Gets the list of <see cref="ChartableComponent"/>s.
        /// </summary>
        /// <returns>A <see cref="IEnumerable{T}"/> list of <see cref="ChartableComponent"/> objects</returns>
        IEnumerable<ChartableComponent> GetOrgChartComponentsWithoutMembers();

        /// <summary>
        /// Gets the list of <see cref="ChartableComponentWithMember"/>s.
        /// </summary>
        /// <returns>A <see cref="IEnumerable{T}"/> list of <see cref="ChartableComponentWithMember"/> objects</returns>
        IEnumerable<ChartableComponentWithMember> GetOrgChartComponentsWithMembers();

        /// <summary>
        /// Gets the list of <see cref="PositionWithMemberCountItem"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> list of <see cref="PositionWithMemberCountItem"/>s/></returns>
        IEnumerable<PositionWithMemberCountItem> GetPositionListWithMemberCount();

        /// <summary>
        /// Adds a <see cref="Position"/> to the Positions collection.
        /// </summary>
        /// <remarks>
        /// This depends on <see cref="OrgChartDemo.Models.ExtensionMethods.ExtensionMethods.Add{T}(IEnumerable{T}, T)"/> to add an item to an <see cref="IEnumerable{T}"/>
        /// </remarks>
        /// <param name="p">A <see cref="Position"/> to add to the Positions collection.</param>
        void AddPosition(Position p);

        /// <summary>
        /// Removes the position.
        /// </summary>
        /// <param name="PositionIdToRemove">The position identifier to remove.</param>
        void RemovePosition(int PositionIdToRemove);

        /// <summary>
        /// Edits the position.
        /// </summary>
        /// <param name="p">The <see cref="Position"/> to edit</param>
        void EditPosition(Position p);

    }
}
