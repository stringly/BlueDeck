using System.Collections.Generic;
using OrgChartDemo.Models.Types;
using System.ComponentModel.DataAnnotations;


namespace OrgChartDemo.Models.ViewModels
{
    /// <summary>
    /// ViewModel used to display a Position and populate a selectlist of Component Names/Ids to facilitate adding a Position or changing the Component to which a position is assigned. 
    /// </summary>
    public class MemberAddEditViewModel
    {
        /// <summary>
        /// Gets or sets the Id of the Member.
        /// </summary>
        /// <value>
        /// The Member's Id.
        /// </value>
        public int? MemberId { get; set; }

        /// <summary>
        /// Gets or sets the Rank of the Member.
        /// </summary>
        [Display(Name = "Rank")]
        public int? MemberRank { get; set; }

        /// <summary>
        /// Gets or sets the member gender.
        /// </summary>
        /// <value>
        /// The member gender.
        /// </value>
        [Required]
        [Display(Name = "Gender")]
        public int? MemberGender { get; set; }

        /// <summary>
        /// Gets or sets the member race.
        /// </summary>
        /// <value>
        /// The member race.
        /// </value>
        [Required]
        [Display(Name = "Race")]
        public int? MemberRace { get; set; }

        /// <summary>
        /// Gets or sets the First Name of the Member.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        [StringLength(50), Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the Last Name of the Member.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        [StringLength(50), Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the Middle Name of the Member.
        /// </summary>
        /// <value>
        /// The Member's middle name.
        /// </value>
        [StringLength(50)]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        /// <summary>
        /// Gets or sets the Member's IDNumber.
        /// </summary>
        /// <remarks>
        /// This is NOT the Member Entity Id. This is the Member's employee Id number
        /// </remarks>
        /// <value>
        /// The Member's Departmental ID Number.
        /// </value>
        [StringLength(50), Required]
        [Display(Name = "ID Number")]
        public string IdNumber { get; set; }

        /// <summary>
        /// Gets or sets the Member's Duty Status.
        /// </summary>
        /// <value>
        /// The Member's Duty Status Index.
        /// </value>
        [Required]
        [Display(Name = "Duty Status")]
        public int? DutyStatusId { get; set; }

        /// <summary>
        /// Gets or sets the Member's email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [StringLength(50), Required]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the Position identifier.
        /// </summary>
        /// <value>
        /// The position identifier.
        /// </value>
        [Display(Name = "Current Assignment")]
        public int? PositionId { get; set; }
                
        public List<MemberContactNumber> ContactNumbers { get; set; } 

        /// <summary>
        /// Gets or sets a list of <see cref="T:OrgChartDemo.Models.Types.MemberRankSelectListItem"/>s.
        /// </summary>
        /// <value>
        /// The rank list.
        /// </value>
        public List<MemberRankSelectListItem> RankList { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="T:OrgChartDemo.Models.Types.PositionSelectListItem"/>.
        /// </summary>
        /// <value>
        /// The positions.
        /// </value>
        public List<PositionSelectListItem> Positions { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="T:OrgChartDemo.Models.Types.MemberGenderSelectListItem"/>.
        /// </summary>
        /// <value>
        /// The genders.
        /// </value>
        public List<MemberGenderSelectListItem> GenderList { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="T:OrgChartDemo.Models.Types.MemberRaceSelectListItem"/>.
        /// </summary>
        /// <value>
        /// The races.
        /// </value>
        public List<MemberRaceSelectListItem> RaceList { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="T:OrgChartDemo.Models.Types.MemberDutyStatusSelectListItem"/>.
        /// </summary>
        /// <value>
        /// The races.
        /// </value>
        public List<MemberDutyStatusSelectListItem> DutyStatus { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:OrgChartDemo.ViewModels.MemberWithPositionListViewModel"/> class.
        /// <remarks>
        /// This parameter-less constructor had to be added in because the <see cref="T:OrgChartDemo.ViewModels.MemberWithPositionListViewModel(Member, List{Position})"/> constructor overrode the default, and the form POST model-binding failed
        /// </remarks>
        /// </summary>
        public MemberAddEditViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberAddEditViewModel"/> class.
        /// </summary>
        /// <param name="_member">A <see cref="T:OrgChartDemo.Models.Member"/>.</param>
        /// <param name="_positionList">A <see cref="T:List{OrgChartDemo.Models.Position}"/>.</param>
        /// <param name="_rankList">A <see cref="T:List{OrgChartDemo.Models.Types.MemberRankSelectListItem}}"/>.</param>
        /// <param name="_genderList">A <see cref="T:List{OrgChartDemo.Models.Types.MemberGenderSelectListItem}}"/>.</param>
        /// <param name="_raceList">A <see cref="T:List{OrgChartDemo.Models.Types.MemberRaceSelectListItem}}"/>.</param>
        /// <param name="_dutyStatusList">A <see cref="T:List{OrgChartDemo.Models.Types.MemberDutyStatusSelectListItem}}"/>.</param>
        public MemberAddEditViewModel(Member _member, 
            List<Position> _positionList, 
            List<MemberRankSelectListItem> _rankList, 
            List<MemberGenderSelectListItem> _genderList, 
            List<MemberRaceSelectListItem> _raceList,
            List<MemberDutyStatusSelectListItem> _dutyStatusList)
        {
            MemberId = _member?.MemberId;
            MemberRank = _member?.Rank?.RankId;
            FirstName = _member.FirstName;
            LastName = _member.LastName;
            MiddleName = _member.MiddleName;
            IdNumber = _member.IdNumber;
            DutyStatusId = _member?.DutyStatus?.DutyStatusId;
            Email = _member.Email;
            PositionId = _member?.Position?.PositionId;
            MemberGender = _member?.Gender?.GenderId;
            MemberRace = _member?.Race?.MemberRaceId;
            ContactNumbers = _member.PhoneNumbers;
            RankList = _rankList;
            GenderList = _genderList;
            RaceList = _raceList;
            DutyStatus = _dutyStatusList;
            Positions = _positionList.ConvertAll(x => new PositionSelectListItem { PositionId = x.PositionId, PositionName = x.Name });

        }
    }
}
