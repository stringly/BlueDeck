using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models
{

    /// <summary>
    /// Base class for creating an Organization Chart using GetOrgChart
    /// </summary>
    public class ChartableComponent
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id of the component.
        /// </value>
        public int id { get; set; }

        /// <summary>
        /// Gets or sets the parentid. Combined with <see cref="ChartableComponent.id"/>, this establishes the Child/Parent relationship for rendering using <a href="http://www.getorgchart.com/Documentation">GetOrgChart</a>.
        /// </summary>
        /// <value>
        /// The parentid of the component.
        /// </value>
        public int? parentid { get; set; }

        /// <summary>
        /// Gets or sets the name of the component.
        /// </summary>
        /// <value>
        /// The name of the component.
        /// </value>
        public string componentName { get; set; }
    }
}
