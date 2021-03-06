﻿using System.Collections.Generic;
using BlueDeck.Models.Types;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System;

namespace BlueDeck.Models.ViewModels
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
        [Required]
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
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only alphanumeric characters are allowed.")]
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
        
        [Display(Name = "Account Status")]
        public int? AppStatusId { get; set; }
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

        /// <summary>
        /// Gets or sets the Position identifier for the Member's Temporary Position.
        /// </summary>
        /// <value>
        /// The position identifier for the member's Temporary (TDY) position.
        /// </value>
        [Display(Name = "TDY Assignment")]
        [PrimaryAndTempPositionNotEqual("PositionId", ErrorMessage = "Temporary Assignment cannot be the same as primary assignment.")]
        public int? TempPositionId { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The Member's display name, used only for Display. This is not a model-bound editable field.
        /// </value>
        public string DisplayName { get; private set; }

        [Display(Name = "Windows Logon Name")]
        [Required]
        public string LDAPName { get; set; }

        [Display(Name = "User")]
        public bool IsUser { get; set; }

        [Display(Name = "Component Admin")]
        public bool IsComponentAdmin { get; set; }

        [Display(Name = "Global Admin")]
        public bool IsGlobalAdmin { get; set; }

        [Display(Name = "Created By")]
        public string Creator { get; set; }
        [Display(Name = "Created")]
        public DateTime? CreatedDate { get; set; }
        [Display(Name = "Last Modified By")]
        public string LastModifiedBy { get; set; }
        [Display(Name = "Last Modified")]
        public DateTime? LastModified { get; set; }
        public int? LastModifiedById { get; set; }
        public int? CreatedById { get; set; }

        /// <summary>
        /// Gets or sets the Member's Contact Numbers.
        /// </summary>
        /// <remarks>
        /// This property stores a List of <see cref="ContactNumber"/> objects to store the Member's phone numbers.
        /// </remarks>
        /// <value>
        /// The contact numbers.
        /// </value>
        [Display(Name = "Contact Numbers")]
        public List<ContactNumber> ContactNumbers { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="T:BlueDeck.Models.Types.MemberRankSelectListItem"/>s.
        /// </summary>
        /// <value>
        /// The rank list.
        /// </value>
        public List<MemberRankSelectListItem> RankList { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="T:BlueDeck.Models.Types.PositionSelectListItem"/>.
        /// </summary>
        /// <value>
        /// The positions.
        /// </value>
        public List<PositionSelectListItem> Positions { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="T:BlueDeck.Models.Types.MemberGenderSelectListItem"/>.
        /// </summary>
        /// <value>
        /// The genders.
        /// </value>
        public List<MemberGenderSelectListItem> GenderList { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="T:BlueDeck.Models.Types.MemberRaceSelectListItem"/>.
        /// </summary>
        /// <value>
        /// The races.
        /// </value>
        public List<MemberRaceSelectListItem> RaceList { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="MemberDutyStatusSelectListItem"/>.
        /// </summary>
        /// <remarks>
        /// This property is used to populate drop-down lists of Member Duty Statuses.
        /// </remarks>
        /// <value>
        /// The races.
        /// </value>
        public List<MemberDutyStatusSelectListItem> DutyStatus { get; set; }

        public List<ApplicationStatusSelectListItem> AppStatuses { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="PhoneNumberTypeSelectListItem"/>.
        /// </summary>
        /// <remarks>
        /// This property is used to populate drop-down lists of Phone Number Types.
        /// </remarks>
        /// <value>
        /// The phone number types.
        /// </value>
        public List<PhoneNumberTypeSelectListItem> PhoneNumberTypes { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberAddEditViewModel"/> class.
        /// </summary>
        public MemberAddEditViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberAddEditViewModel"/> class.
        /// </summary>
        /// <param name="_member">A <see cref="Member"/>.</param>
        /// <param name="_positionList">A <see cref="PositionSelectListItem"/>.</param>
        /// <param name="_rankList">A <see cref="MemberRankSelectListItem"/>.</param>
        /// <param name="_genderList">A <see cref="MemberGenderSelectListItem"/>.</param>
        /// <param name="_raceList">A <see cref="MemberRaceSelectListItem"/>.</param>
        /// <param name="_dutyStatusList">A <see cref="MemberDutyStatusSelectListItem"/>.</param>
        /// <param name="_phoneNumberTypes">A <see cref="PhoneNumberTypeSelectListItem"/>.</param>
        /// <param name="_appStatusList">A List of  <see cref="ApplicationStatusSelectListItem"/>.</param>
        public MemberAddEditViewModel(Member _member, 
            List<PositionSelectListItem> _positionList, 
            List<MemberRankSelectListItem> _rankList, 
            List<MemberGenderSelectListItem> _genderList, 
            List<MemberRaceSelectListItem> _raceList,
            List<MemberDutyStatusSelectListItem> _dutyStatusList,
            List<PhoneNumberTypeSelectListItem> _phoneNumberTypes,
            List<ApplicationStatusSelectListItem> _appStatusList)
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
            TempPositionId = _member?.TempPositionId;
            MemberGender = _member?.Gender?.GenderId;
            MemberRace = _member?.Race?.MemberRaceId;
            ContactNumbers = _member?.PhoneNumbers.ToList() ?? new List<ContactNumber>();
            RankList = _rankList;
            GenderList = _genderList;
            RaceList = _raceList;
            DutyStatus = _dutyStatusList;
            AppStatusId = _member?.AppStatusId ?? 1;
            PhoneNumberTypes = _phoneNumberTypes;
            DisplayName = _member.GetTitleName();
            LDAPName = _member.LDAPName;
            Positions = _positionList;
            AppStatuses = _appStatusList;
            Creator = _member.Creator?.GetTitleName() ?? "";
            CreatedById = _member?.CreatorId;
            CreatedDate = _member?.CreatedDate;
            LastModifiedBy = _member?.LastModifiedBy?.GetTitleName() ?? "";
            LastModifiedById = _member?.LastModifiedById;
            LastModified = _member?.LastModified;
            IsUser = _member?.CurrentRoles?.Any(x => x.RoleType.RoleTypeName == "User") ?? false;
            IsComponentAdmin = _member?.CurrentRoles?.Any(x => x.RoleType.RoleTypeName == "ComponentAdmin") ?? false;
            IsGlobalAdmin = _member?.CurrentRoles?.Any(x => x.RoleType.RoleTypeName == "GlobalAdmin") ?? false;
           

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberAddEditViewModel"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor serves the Modal Form to add a Member which is invoked from the RosterManager view.
        /// The Member's Position cannot be changed from this Modal, so there is no List of PositionSelectListItem parameter.
        /// </remarks>
        /// <param name="_member">The member.</param>
        /// <param name="_rankList">A List of <see cref="MemberRankSelectListItem"/>.</param>
        /// <param name="_genderList">A List of  <see cref="MemberGenderSelectListItem"/>.</param>
        /// <param name="_raceList">A List of  <see cref="MemberRaceSelectListItem"/>.</param>
        /// <param name="_dutyStatusList">A List of  <see cref="MemberDutyStatusSelectListItem"/>.</param>
        /// <param name="_phoneNumberTypes">A List of  <see cref="PhoneNumberTypeSelectListItem"/>.</param>
        /// <param name="_appStatusList">A List of  <see cref="ApplicationStatusSelectListItem"/>.</param>
        public MemberAddEditViewModel(Member _member,
            List<MemberRankSelectListItem> _rankList,
            List<MemberGenderSelectListItem> _genderList,
            List<MemberRaceSelectListItem> _raceList,
            List<MemberDutyStatusSelectListItem> _dutyStatusList,
            List<PhoneNumberTypeSelectListItem> _phoneNumberTypes,
            List<ApplicationStatusSelectListItem> _appStatusList)
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
            TempPositionId = _member?.TempPositionId;
            MemberGender = _member?.Gender?.GenderId;
            MemberRace = _member?.Race?.MemberRaceId;
            ContactNumbers = _member?.PhoneNumbers.ToList() ?? new List<ContactNumber>();
            RankList = _rankList;
            GenderList = _genderList;
            RaceList = _raceList;
            DutyStatus = _dutyStatusList;
            AppStatusId = _member?.AppStatusId ?? 1;
            PhoneNumberTypes = _phoneNumberTypes;
            DisplayName = _member.GetTitleName();
            LDAPName = _member?.LDAPName;
            AppStatuses = _appStatusList;
            Creator = _member.Creator?.GetTitleName() ?? "";
            CreatedById = _member?.CreatorId;
            CreatedDate = _member?.CreatedDate;
            LastModifiedBy = _member.LastModifiedBy?.GetTitleName() ?? "";
            LastModifiedById = _member?.LastModifiedById;
            LastModified = _member?.LastModified;
            IsUser = _member?.CurrentRoles?.Any(x => x.RoleType.RoleTypeName == "User") ?? false;
            IsComponentAdmin = _member?.CurrentRoles?.Any(x => x.RoleType.RoleTypeName == "ComponentAdmin") ?? false;
            IsGlobalAdmin = _member?.CurrentRoles?.Any(x => x.RoleType.RoleTypeName == "GlobalAdmin") ?? false;
           
        }

    }
    public class PrimaryAndTempPositionNotEqualAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public PrimaryAndTempPositionNotEqualAttribute(string comparisonProperty)
        {
             _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            ErrorMessage = ErrorMessageString;            
            var currentValue = (int)value;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (property == null)
                throw new ArgumentException("Property with this name not found");

            var comparisonValue = (int)property.GetValue(validationContext.ObjectInstance);

            if (currentValue == comparisonValue)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
