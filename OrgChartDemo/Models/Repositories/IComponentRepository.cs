using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models.Repositories
{
    public interface IComponentRepository : IRepository<Component>
    {
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
    }
}
