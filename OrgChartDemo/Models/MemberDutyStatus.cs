using System.ComponentModel.DataAnnotations;

namespace OrgChartDemo.Models.Types
{
    /// <summary>
    /// A Class that represents a member's duty status.  Contains properties and methods used in displaying the duty status of a <see cref="T:OrgChartDemo.Models.Member"/>
    /// </summary>
    public class MemberDutyStatus
    {
        /// <summary>
        /// Gets or sets the duty status identifier.
        /// </summary>
        /// <value>
        /// The duty status identifier.
        /// </value>
        [Key]
        public int DutyStatusId { get; set; }

        /// <summary>
        /// Gets or sets the name of the duty status.
        /// </summary>
        /// <value>
        /// The name of the duty status.
        /// </value>
        public string DutyStatusName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has police power.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has police power; otherwise, <c>false</c>.
        /// </value>
        public bool HasPolicePower { get; set; }

        /// <summary>
        /// Gets or sets the abbreviation.
        /// </summary>
        /// <value>
        /// The abbreviation.
        /// </value>
        public char Abbreviation { get; set; }
    }
}
