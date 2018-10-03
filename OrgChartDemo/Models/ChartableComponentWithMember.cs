using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models {

    /// <summary>
    /// An extension of <see cref="T:OrgChartDemo.Models.ChartableComponent"/> that adds additional display information to render in a <a href="http://www.getorgchart.com/Documentation">GetOrgChart</a> org chart.
    /// </summary>
    /// <remarks>
    /// This is not a direct representation of a <see cref="T:OrgChartDemo.Models.Component"/> entity. A <a href="http://www.getorgchart.com/Documentation">
    /// GetOrgChart</a> cannot render a component with multiple positions or multiple members.
    /// This class is used to map GetOrgChart Components, NOT domain <see cref="T:OrgChartDemo.Models.Component"/>s.
    /// When rendering <see cref="T:OrgChartDemo.Models.Member"/> information into GetOrgChart, an instance of this class will be generated for each.
    /// </remarks>
    /// <seealso cref="T:OrgChartDemo.Models.ChartableComponent" />
    public class ChartableComponentWithMember : ChartableComponent {
        /// <summary>
        /// Gets or sets the position id.
        /// </summary>
        /// <value>
        /// The position identifier.
        /// </value>
        public int PositionId { get; set; }
        
        /// <summary>
        /// Gets or sets the name of the position.
        /// </summary>
        /// <value>
        /// The name of the position.
        /// </value>
        public string PositionName { get; set; }

        /// <summary>
        /// Gets or sets the member identifier.
        /// </summary>
        /// <value>
        /// The member identifier.
        /// </value>
        public int? MemberId { get; set; }

        /// <summary>
        /// Gets or sets the name of the member.
        /// </summary>
        /// <value>
        /// The name of the member.
        /// </value>
        public string MemberName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the contact number.
        /// </summary>
        /// <value>
        /// The contact number.
        /// </value>
        public string ContactNumber { get; set; }        
    }
}
