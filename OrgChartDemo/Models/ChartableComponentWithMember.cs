using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models {

    /// <summary>
    /// An extension of <see cref="ChartableComponent"/> that adds additional display information to render in a <a href="http://www.getorgchart.com/Documentation">GetOrgChart</a> org chart.
    /// </summary>
    /// <remarks>
    /// This is not a direct representation of a <see cref="Component"/> entity. A <a href="http://www.getorgchart.com/Documentation">
    /// GetOrgChart</a> cannot render a component with multiple positions or multiple members.
    /// This class is used to map GetOrgChart Components, NOT domain <see cref="Component"/>s.
    /// When rendering <see cref="Member"/> information into GetOrgChart, an instance of this class will be generated for each.
    /// </remarks>
    /// <seealso cref="OrgChartDemo.Models.ChartableComponent" />
    public class ChartableComponentWithMember : ChartableComponent {
        /// <summary>
        /// Gets or sets the position id.
        /// </summary>
        /// <value>
        /// The position identifier.
        /// </value>
        public int positionId { get; set; }
        
        /// <summary>
        /// Gets or sets the name of the position.
        /// </summary>
        /// <value>
        /// The name of the position.
        /// </value>
        public string positionName { get; set; }

        /// <summary>
        /// Gets or sets the member identifier.
        /// </summary>
        /// <value>
        /// The member identifier.
        /// </value>
        public int? memberId { get; set; }

        /// <summary>
        /// Gets or sets the name of the member.
        /// </summary>
        /// <value>
        /// The name of the member.
        /// </value>
        public string memberName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string email { get; set; }

        /// <summary>
        /// Gets or sets the contact number.
        /// </summary>
        /// <value>
        /// The contact number.
        /// </value>
        public string contactNumber { get; set; }        
    }
}
