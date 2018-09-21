using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using OrgChartDemo.Models.Types;

namespace OrgChartDemo.Models {
    /// <summary>
    /// Member Entity
    /// </summary>
    public class Member {

        /// <summary>
        /// Gets or sets the Member's Id (PK).
        /// </summary>
        /// <value>
        /// The Member's Id (PK).
        /// </value>
        [Key]
        public int MemberId { get; set; }

        /// <summary>
        /// Gets or sets the Member's <see cref="Rank"/>.
        /// </summary>
        /// <value>
        /// The <see cref="Rank"/> of the Member
        /// </value>
        public MemberRank Rank { get; set; }

        /// <summary>
        /// Gets or sets the Member's first name.
        /// </summary>
        /// <value>
        /// The first name of the Member.
        /// </value>
        public string FirstName{ get; set; }

        /// <summary>
        /// Gets or sets the Member's last name.
        /// </summary>
        /// <value>
        /// The last name of the Member.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the Member's middle name.
        /// </summary>
        /// <value>
        /// The name of the middle.
        /// </value>
        public string MiddleName { get; set; }

        /// <summary>
        /// Gets or sets the Member's Departmental Id Number.
        /// </summary>
        /// <value>
        /// The Departmental Id Number of the Member.
        /// </value>
        public string IdNumber { get; set; }

        /// <summary>
        /// Gets or sets the Member's email.
        /// </summary>
        /// <value>
        /// The Member's email.
        /// </value>
        public string Email {get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Position"/> to which the Member is assigned.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public virtual Position Position { get; set; }

        /// <summary>
        /// Gets the formal title form of the Member's name and rank.
        /// </summary>
        /// <remarks>
        /// e.g. "POFC Foo Bar #1234"
        /// </remarks>
        /// <returns>A <see cref="string"/> with the formal display name for the Member</returns>
        public string GetTitleName() => $"{this.Rank.RankShort}. {this.FirstName} {this.LastName} #{this.IdNumber}";

        /// <summary>
        /// Gets the Member's name in "LastName, FirstName" format.
        /// </summary>
        /// <returns>A <see cref="string"/> with the Member's "LastName, FirstName"</returns>
        public string GetLastNameFirstName() => $"{this.LastName}, {this.FirstName}";
                
    }
}

