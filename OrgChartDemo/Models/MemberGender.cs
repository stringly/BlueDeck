using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models.Types
{
    /// <summary>
    /// A Class that represents a Gender.  Contains properties and methods used in displaying the gender of a <see cref="T:OrgChartDemo.Models.Member"/>
    /// </summary>
    public class MemberGender
    {
        /// <summary>
        /// Gets or sets the gender's Id.
        /// </summary>
        /// <value>
        /// The gender Id.
        /// </value>
        [Key]
        public int GenderId { get; set; }

        /// <summary>
        /// Gets or sets the full name of the gender.
        /// </summary>
        /// <value>
        /// The full name of the gender.
        /// </value>
        public string GenderFullName { get; set; }

        /// <summary>
        /// Gets or sets the gender's single-character abbreviation.
        /// </summary>
        /// <value>
        /// The gender's single-character abbreviation.
        /// </value>
        public char Abbreviation { get; set; }
    }
}
