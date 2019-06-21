using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlueDeck.Models.Enums
{
    /// <summary>
    /// Enumeration Class for Application Statuses
    /// </summary>
    public class AppStatus
    {
        /// <summary>
        /// Gets or sets the application status identifier.
        /// </summary>
        /// <value>
        /// The application status identifier.
        /// </value>
        [Key]
        [Display(Name = "Status Id")]
        public int? AppStatusId { get; set; }

        /// <summary>
        /// Gets or sets the name of the status.
        /// </summary>
        /// <value>
        /// The name of the status.
        /// </value>
        [Display(Name = "Status Name")]
        [Required]
        public string StatusName { get; set; }

        /// <summary>
        /// Gets or sets the list of members that are in this status.
        /// </summary>
        /// <value>
        /// The members.
        /// </value>
        public virtual IEnumerable<Member> Members { get; set; }
        
    }
}
