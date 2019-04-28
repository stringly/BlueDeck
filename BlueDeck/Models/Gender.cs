using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models {
    /// <summary>
    /// A Class that represents a Gender.  Contains properties and methods used in displaying the gender of a <see cref="T:BlueDeck.Models.Member"/>
    /// </summary>
    public class Gender
    {
        /// <summary>
        /// Gets or sets the gender's Id.
        /// </summary>
        /// <value>
        /// The gender Id.
        /// </value>
        [Key]
        public int? GenderId { get; set; }

        /// <summary>
        /// Gets or sets the full name of the gender.
        /// </summary>
        /// <value>
        /// The full name of the gender.
        /// </value>
        [Display(Name = "Full Name")]
        [Required]
        public string GenderFullName { get; set; }

        /// <summary>
        /// Gets or sets the gender's single-character abbreviation.
        /// </summary>
        /// <value>
        /// The gender's single-character abbreviation.
        /// </value>
        [Display(Name = "Abbreviation")]
        [Required]
        public char Abbreviation { get; set; }

        public virtual IEnumerable<Member> Members { get; set; }

    }
}
