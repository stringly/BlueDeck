using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.Types
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
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the parentid. Combined with <see cref="T:BlueDeck.Models.ChartableComponent.Id"/>, this establishes the Child/Parent relationship for rendering using <a href="http://www.getorgchart.com/Documentation">GetOrgChart</a>.
        /// </summary>
        /// <value>
        /// The parentid of the component.
        /// </value>
        public int? Parentid { get; set; }

        /// <summary>
        /// Gets or sets the name of the component.
        /// </summary>
        /// <value>
        /// The name of the component.
        /// </value>
        public string ComponentName { get; set; }
        public string CustomHTML { get; set; }
    }
}
