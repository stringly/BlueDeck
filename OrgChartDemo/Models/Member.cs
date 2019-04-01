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

        public int RankId { get; set; }
        /// <summary>
        /// Gets or sets the Member's <see cref="Rank"/>.
        /// </summary>
        /// <value>
        /// The <see cref="Rank"/> of the Member
        /// </value>
        [Display(Name = "Rank")]
        public virtual Rank Rank { get; set; }

        /// <summary>
        /// Gets or sets the Member's first name.
        /// </summary>
        /// <value>
        /// The first name of the Member.
        /// </value>
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the Member's last name.
        /// </summary>
        /// <value>
        /// The last name of the Member.
        /// </value>
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the Member's middle name.
        /// </summary>
        /// <value>
        /// The name of the middle.
        /// </value>
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        /// <summary>
        /// Gets or sets the Member's Departmental Id Number.
        /// </summary>
        /// <value>
        /// The Departmental Id Number of the Member.
        /// </value>
        [Display(Name = "ID Number")]
        public string IdNumber { get; set; }

        public int GenderId { get; set; }
        /// <summary>
        /// Gets or sets the Member's gender.
        /// </summary>
        /// <value>
        /// The Member's gender.
        /// </value>
        /// <seealso cref="T:OrgChartDemo.Models.Types.MemberGender"/>
        [Display(Name = "Gender")]
        public virtual Gender Gender { get; set; }

        public int RaceId { get; set; }
        /// <summary>
        /// Gets or sets the Member's race.
        /// </summary>
        /// <value>
        /// The Member's race.
        /// </value>
        /// <seealso cref="T:OrgChartDemo.Models.Types.MemberRace"/>
        [Display(Name = "Race")]
        public virtual Race Race { get; set; }

        public int DutyStatusId {get;set;}
        /// <summary>
        /// Gets or sets the Member's duty status.
        /// </summary>
        /// <value>
        /// The Member's race.
        /// </value>
        /// <seealso cref="T:OrgChartDemo.Models.Types.MemberDutyStatus"/>
        [Display(Name = "Duty Status")]
        public virtual DutyStatus DutyStatus { get; set; }

        public int? AppStatusId { get;set; }

        public AppStatus AppStatus { get;set; }
        /// <summary>
        /// Gets or sets the Member's email.
        /// </summary>
        /// <value>
        /// The Member's email.
        /// </value>
        [Display(Name = "Email Address")]
        public string Email {get; set; }

        [Display(Name = "Windows Logon Name")]
        public string LDAPName {get; set;}

        public int PositionId { get; set; }
        /// <summary>
        /// Gets or sets the <see cref="Position"/> to which the Member is assigned.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        [Display(Name = "Current Assignment")]
        public virtual Position Position { get; set; }

        [Display(Name = "Contact Numbers")]
        public ICollection<ContactNumber> PhoneNumbers { get; set; }

        [Display(Name = "Current Roles")]
        public virtual ICollection<Role> CurrentRoles { get; set; }

        public Member()
        {
            PhoneNumbers = new List<ContactNumber>();
        }
        /// <summary>
        /// Gets the formal title form of the Member's name and rank.
        /// </summary>
        /// <remarks>
        /// e.g. "POFC Foo Bar #1234"
        /// </remarks>
        /// <returns>A <see cref="string"/> with the formal display name for the Member</returns>
        public string GetTitleName()
        {
            if(MemberId == 0)
            {
                return "New Member";
            }
            else if(Rank != null && FirstName != null && LastName != null && IdNumber != null)
            {
                return $"{this.Rank.RankShort} {this.FirstName} {this.LastName} #{this.IdNumber}";
            }
            else if (FirstName != null && LastName != null)
            {
                return $"{LastName}, {FirstName}";
            }
            else
            {
                return $"BlueDeck Member #{MemberId}";
            }     
            
        }

        /// <summary>
        /// Gets the Member's name in "LastName, FirstName" format.
        /// </summary>
        /// <returns>A <see cref="string"/> with the Member's "LastName, FirstName"</returns>
        public string GetLastNameFirstName() => $"{this.LastName}, {this.FirstName}";
                
    }
}

