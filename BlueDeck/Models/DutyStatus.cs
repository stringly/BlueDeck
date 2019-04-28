using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlueDeck.Models
{
    /// <summary>
    /// A Class that represents a member's duty status.  Contains properties and methods used in displaying the duty status of a <see cref="T:BlueDeck.Models.Member"/>
    /// </summary>
    public class DutyStatus
    {
        /// <summary>
        /// Gets or sets the duty status identifier.
        /// </summary>
        /// <value>
        /// The duty status identifier.
        /// </value>
        [Key]
        [Display(Name = "Status Id")]
        public int? DutyStatusId { get; set; }

        /// <summary>
        /// Gets or sets the name of the duty status.
        /// </summary>
        /// <value>
        /// The name of the duty status.
        /// </value>
        [Display(Name = "Status Name")]
        [Required]
        public string DutyStatusName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has police power.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has police power; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "Has Police Power")]
        public bool HasPolicePower { get; set; }

        /// <summary>
        /// Gets or sets the abbreviation.
        /// </summary>
        /// <value>
        /// The abbreviation.
        /// </value>
        [Display(Name = "Abbreviation")]
        [Required]
        public char Abbreviation { get; set; }

        public virtual IEnumerable<Member> Members { get; set; }
    }
}
