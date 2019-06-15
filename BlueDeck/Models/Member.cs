using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using BlueDeck.Models.Types;

namespace BlueDeck.Models {
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
        /// Gets or sets the Member's <see cref="Rank"/> Id.
        /// </summary>
        /// <value>
        /// The rank identifier.
        /// </value>
        public int RankId { get; set; }
        /// <summary>
        /// Gets or sets the Member's <see cref="Rank"/>.
        /// </summary>
        /// <value>
        /// The <see cref="Rank"/> of the Member
        /// </value>
        [Display(Name = "Rank")]
        [ForeignKey("RankId")]
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

        /// <summary>
        /// Gets or sets the Member's <see cref="Gender"/> Id Number.
        /// </summary>
        /// <value>
        /// The gender identifier.
        /// </value>
        public int GenderId { get; set; }

        /// <summary>
        /// Gets or sets the Member's <see cref="Gender"/>.
        /// </summary>
        /// <remarks>
        /// This is an EF navigation property that must be manually loaded if using Lazy Loading.
        /// </remarks>
        /// <value>
        /// The Member's gender.
        /// </value>
        /// <seealso cref="Gender"/>
        [Display(Name = "Gender")]
        [ForeignKey("GenderId")]
        public virtual Gender Gender { get; set; }

        /// <summary>
        /// Gets or sets the Member's <see cref="Race"/> Id Number.
        /// </summary>
        /// <value>
        /// The race identifier.
        /// </value>
        public int RaceId { get; set; }

        /// <summary>
        /// Gets or sets the Member's race.
        /// </summary>
        /// <value>
        /// The Member's race.
        /// </value>
        /// <seealso cref="Race"/>
        [Display(Name = "Race")]
        public virtual Race Race { get; set; }

        /// <summary>
        /// Gets or sets the Member's <see cref="DutyStatus"/> Id.
        /// </summary>
        /// <value>
        /// The duty status identifier.
        /// </value>
        public int DutyStatusId {get;set;}

        /// <summary>
        /// Gets or sets the Member's <see cref="DutyStatus"/>
        /// </summary>
        /// <value>
        /// The Member's current Duty Status.
        /// </value>
        /// <seealso cref="DutyStatus"/>
        [Display(Name = "Duty Status")]
        [ForeignKey("DutyStatusId")]
        public virtual DutyStatus DutyStatus { get; set; }

        /// <summary>
        /// Gets or sets the Member's <see cref="AppStatus"/> Id.
        /// </summary>
        /// <value>
        /// The application status identifier.
        /// </value>
        [Display(Name = "Account Status")]
        public int? AppStatusId { get;set; }

        /// <summary>
        /// Gets or sets the Member's <see cref="AppStatus"/>.
        /// </summary>
        /// <remarks>
        /// This is an EF navigation property.
        /// </remarks>
        /// <value>
        /// The application status.
        /// </value>
        [ForeignKey("AppStatusId")]
        public AppStatus AppStatus { get;set; }

        /// <summary>
        /// Gets or sets the Member's email.
        /// </summary>
        /// <value>
        /// The Member's email.
        /// </value>
        [Display(Name = "Email Address")]
        public string Email {get; set; }

        /// <summary>
        /// Gets or sets the Member's LDAP Name.
        /// </summary>
        /// <remarks>
        /// The LDAP name is essential, as it is what BlueDeck uses to identify Windows users when they log on.
        /// </remarks>
        /// <value>
        /// The name of the LDAP.
        /// </value>
        [Display(Name = "Windows Logon Name")]
        [Required]
        public string LDAPName {get; set;}

        /// <summary>
        /// Gets or sets the Member's payroll identifier.
        /// </summary>
        /// <remarks>
        /// As of Version 1, this is the Stromberg ETS Id Number for the Member.
        /// This may change when the new Payroll System goes live.
        /// </remarks>
        /// <value>
        /// The payroll identifier.
        /// </value>
        [Display(Name = "Payroll ID")]
        [Required]
        public string PayrollID { get; set; }

        /// <summary>
        /// Gets or sets the date that the Member was hired by the Organization.
        /// </summary>
        /// <value>
        /// The hire date.
        /// </value>
        [Display(Name = "Hire Date")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime HireDate { get; set; }

        /// <summary>
        /// Gets or sets the Member's Organizational position number.
        /// </summary>
        /// <remarks>
        /// This is distinct from the Member's PositionId. The Organization assigns position numbers to Employees based on rules that I don't full understand.
        /// For now, I'm going to treat them as Member-specific.
        /// </remarks>
        /// <value>
        /// The org position number.
        /// </value>
        [Display(Name = "Position Number")]
        public string OrgPositionNumber { get; set; }

        /// <summary>
        /// Gets or sets the MemberID of the Member who created this Member
        /// </summary>
        /// <value>
        /// The creator identifier.
        /// </value>
        public int? CreatorId { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Member"/> who created this Member
        /// </summary>
        /// <remarks>
        /// EF Navigation property
        /// </remarks>
        /// <value>
        /// The creator.
        /// </value>
        [ForeignKey("CreatorId")]
        [Display(Name = "Created By")]
        public virtual Member Creator { get; set; }

        /// <summary>
        /// Gets or sets the date that this Member was created.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        [Display(Name = "Date Created")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets date that this member was last modified.
        /// </summary>
        /// <value>
        /// The last modified.
        /// </value>
        [Display(Name = "Date Last Modified")]
        public DateTime LastModified { get; set; }

        /// <summary>
        /// Gets or sets the MemberId of the <see cref="Member"/> who last modified this Member.
        /// </summary>
        /// <value>
        /// The last modified by identifier.
        /// </value>
        public int? LastModifiedById { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Member"/> who last modified this Member.
        /// </summary>
        /// <remarks>
        /// EF navigation property.
        /// </remarks>
        /// <value>
        /// The <see cref="Member"/> who last modified this Member.
        /// </value>
        [ForeignKey("LastModifiedById")]
        [Display(Name = "Last Modified By")]
        public virtual Member LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the position identifier.
        /// </summary>
        /// <value>
        /// The position identifier.
        /// </value>
        public int PositionId { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Position"/> to which the Member is assigned.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        [Display(Name = "Current Assignment")]
        [ForeignKey("PositionId")]
        public Position Position { get; set; }

        /// <summary>
        /// Gets or sets the temporary <see cref="Position"/> position identifier.
        /// </summary>
        /// <remarks>
        /// A member can be assigned a Temporary (TDY) position, which allows them to be included in the roster
        /// (but not demographic or staffing) calculations of a second position while preserving their primary position.
        /// </remarks>
        /// <value>
        /// The temporary position identifier.
        /// </value>
        public int? TempPositionId { get; set; }

        /// <summary>
        /// Gets or sets the temporary duty position.
        /// </summary>
        /// <value>
        /// The temporary duty position.
        /// </value>
        [Display(Name = "TDY Assignment")]
        [ForeignKey("TempPositionId")]
        public Position TempPosition {get;set;}

        /// <summary>
        /// Gets or sets the Member's <see cref="PhoneNumbers"/> collection.
        /// </summary>
        /// <value>
        /// The Member's Phone Numbers collection
        /// </value>
        [Display(Name = "Contact Numbers")]
        public ICollection<ContactNumber> PhoneNumbers { get; set; } = new List<ContactNumber>();

        /// <summary>
        /// Gets or sets the Member's current <see cref="Role"/> collection
        /// </summary>
        /// <value>
        /// The Member's current <see cref="Role"/> collection.
        /// </value>
        [Display(Name = "Current Roles")]
        public virtual ICollection<Role> CurrentRoles { get; set; } = new List<Role>();

        /// <summary>
        /// Gets or sets the collection of <see cref="Member"/>s created by this Member
        /// </summary>
        /// <value>
        /// The collection of <see cref="Member"/>s that this Member has created.
        /// </value>
        public virtual ICollection<Member> CreatedMembers { get; set; } = new List<Member>();

        /// <summary>
        /// Gets or sets the collection of <see cref="Position"/>s that this Member has created..
        /// </summary>
        /// <value>
        /// The created positions.
        /// </value>
        public virtual ICollection<Position> CreatedPositions { get; set; } = new List<Position>();

        /// <summary>
        /// Gets or sets a collection of <see cref="Component"/> that this Member has created.
        /// </summary>
        /// <value>
        /// The created components.
        /// </value>
        public virtual ICollection<Component> CreatedComponents { get; set; } = new List<Component>();

        /// <summary>
        /// Gets or sets the collection of <see cref="Member"/> that this Member has Last Modified.
        /// </summary>
        /// <value>
        /// The last modified members.
        /// </value>
        public virtual ICollection<Member> LastModifiedMembers { get; set; } = new List<Member>();

        /// <summary>
        /// Gets or sets the collection of <see cref="Position"/> that this Member has last modified.
        /// </summary>
        /// <value>
        /// The last modified positions.
        /// </value>
        public virtual ICollection<Position> LastModifiedPositions { get; set; } = new List<Position>();

        /// <summary>
        /// Gets or sets the collection of <see cref="Component"/> that this Member has last Modified.
        /// </summary>
        /// <value>
        /// The last modified components.
        /// </value>
        public virtual ICollection<Component> LastModifiedComponents { get; set; } = new List<Component>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Member"/> class.
        /// </summary>
        public Member()
        {            
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
        public string GetLastNameFirstName()
        {
            if (!String.IsNullOrEmpty(FirstName) && !String.IsNullOrEmpty(LastName))
            {
                return $"{this.LastName}, {this.FirstName}";
            }
            else
            {
                return "";
            }
            
        }

        /// <summary>
        /// Determines whether the Member has the ComponentAdmin role.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the Member is in the ComponentAdmin role; otherwise, <c>false</c>.
        /// </returns>
        public bool IsComponentAdmin()
        {
            return CurrentRoles.Any(x => x.RoleType.RoleTypeName == "ComponentAdmin");
        }

    }
}

