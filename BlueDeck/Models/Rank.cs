using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrgChartDemo.Models
{
    /// <summary>
    /// A Class that represents a Organizational Rank.  Contains properties and methods used in displaying the rank of a <see cref="T:OrgChartDemo.Models.Member"/>
    /// </summary>
    public class Rank {

        /// <summary>
        /// Gets or sets the rank Id.
        /// </summary>
        /// <value>
        /// The rank Id.
        /// </value>
        [Key]
        public int? RankId { get; set; }

        /// <summary>
        /// Gets or sets the full name of the rank.
        /// </summary>
        /// <remarks>
        /// e.g. "Police Officer First Class" 
        /// </remarks>
        /// <value>
        /// The full name of the rank.
        /// </value>
        [Display(Name = "Rank Name")]
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
        [Display(Name = "Abbreviation")]
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
        [Display(Name = "Pay Grade")]
        [Required]
        public string PayGrade { get; set; }
        [Display(Name = "Is Sworn")]
        public bool IsSworn { get; set; }

        public virtual IEnumerable<Member> Members { get; set; }
        
        public string GetRankImageSource()
        {
            switch (this.RankId)
            {
                case 1:
                    return "";                    
                case 2:
                    return "/lib/bluedeck/css/images/rankicons/L02.png";
                case 3:
                    return "/lib/bluedeck/css/images/rankicons/L03.png";
                case 4:
                    return "/lib/bluedeck/css/images/rankicons/L04.png";
                case 5:
                    return "/lib/bluedeck/css/images/rankicons/L05.png";
                case 6:
                    return "/lib/bluedeck/css/images/rankicons/L06.png";
                case 7:
                    return "/lib/bluedeck/css/images/rankicons/L07.png";
                case 8:
                case 9:
                case 10:
                    return "/lib/bluedeck/css/images/rankicons/L08.png";
                default:
                    return "";                 
            }

        }
    }
}
