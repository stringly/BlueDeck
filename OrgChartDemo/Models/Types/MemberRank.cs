using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models.Types {
    /// <summary>
    /// A Class that represents a Organizational Rank.  Contains properties and methods used in displaying the rank of a <see cref="Member"/>
    /// </summary>
    public class MemberRank {

        /// <summary>
        /// Gets or sets the rank Id.
        /// </summary>
        /// <value>
        /// The rank Id.
        /// </value>
        [Key]
        public int RankId { get; set; }

        /// <summary>
        /// Gets or sets the full name of the rank.
        /// </summary>
        /// <remarks>
        /// e.g. "Police Officer First Class" 
        /// </remarks>
        /// <value>
        /// The full name of the rank.
        /// </value>
        public string RankFullName { get; set; }

        /// <summary>
        /// Gets or sets the rank abbreviation.
        /// </summary>
        /// <remarks>
        /// e.g. "POFC"
        /// </remarks>
        /// <value>
        /// The rank abbreviation.
        /// </value>
        public string RankShort { get; set; }

        /// <summary>
        /// Gets or sets the pay grade for a rank.
        /// </summary>
        /// <remarks>
        /// e.g. "L02"
        /// </remarks>
        /// <value>
        /// The pay grade for the rank.
        /// </value>
        public string PayGrade { get; set; }
    }
}
