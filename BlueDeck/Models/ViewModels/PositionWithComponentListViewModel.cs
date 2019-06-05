using System.Collections.Generic;
using BlueDeck.Models.Types;
using System.ComponentModel.DataAnnotations;
using System;

namespace BlueDeck.Models.ViewModels
{
    /// <summary>
    /// ViewModel used to display a Position and populate a selectlist of Component Names/Ids to facilitate adding a Position or changing the Component to which a position is assigned. 
    /// </summary>
    public class PositionWithComponentListViewModel
    {
        /// <summary>
        /// Gets or sets the Id of the Position.
        /// </summary>
        /// <value>
        /// The Position's Id.
        /// </value>
        public int? PositionId { get; set; }

        /// <summary>
        /// Gets or sets the name of the position.
        /// </summary>
        /// <value>
        /// The name of the position.
        /// </value>
        [StringLength(75), Required]
        [Display(Name = "Position Name")]
        public string PositionName { get; set; }

        /// <summary>
        /// Gets or sets the Id of the Position's parent <see cref="BlueDeck.Models.Component"/>
        /// </summary>
        /// <value>
        /// The parent's ComponentId.
        /// </value>
        [Required]
        [Display(Name = "Parent Component")] 
        public int? ParentComponentId { get; set; }

        /// <summary>
        /// Gets or sets the job title.
        /// </summary>
        /// <value>
        /// The job title.
        /// </value>
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this Position is designated as the manager of it's parent <see cref="BlueDeck.Models.Component"/>.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is manager; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "Manager of Component")]
        [ManageOrAssistantNotBoth("IsAssistantManager", ErrorMessage = "Position cannot be both Manager and Assistant Manager")]
        public bool IsManager { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this position is an assistant manager for it's component <see cref="BlueDeck.Models.Component"/>.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is assistant manager; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "Assistant Manager for Component")]
        [ManageOrAssistantNotBoth("IsManager", ErrorMessage = "Position cannot be both Assistant Manager and Manager")]
        public bool IsAssistantManager { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is unique, or if it can be assigned multiple <see cref="BlueDeck.Models.Member"/>s.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is unique; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "Unique")]
        public bool IsUnique { get; set; }
        /// <summary>
        /// Gets or sets the lineup position.
        /// </summary>
        /// <remarks>
        /// The Lineup position determines the order in which a Position is listed among it's siblings. 
        /// </remarks>
        /// <value>
        /// The lineup position.
        /// </value>
        public int? LineupPosition { get; set; }
        /// <summary>
        /// Gets or sets the callsign.
        /// </summary>
        /// <remarks>
        /// The Callsign is an optional field indicating the radio identifier for a person assigned to this position.
        /// </remarks>
        /// <value>
        /// The callsign.
        /// </value>
        [Display(Name = "Call Sign")]
        [StringLength(20)]
        public string Callsign { get; set; }

        /// <summary>
        /// Gets or sets the creator.
        /// </summary>
        /// <remarks>
        /// This property is the display name of the <see cref="Member"/> that created the Position
        /// </remarks>
        /// <value>
        /// The creator.
        /// </value>
        [Display(Name = "Created By")]
        public string Creator { get; set; }

        /// <summary>
        /// Gets or sets the date that the Member was initially created.
        /// </summary>
        /// <remarks>
        /// This value should be set at initial creation and should never change.
        /// </remarks>
        /// <value>
        /// The created date.
        /// </value>
        [Display(Name = "Date Created")]
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the MemberId of the <see cref="Member"/> who created the Position.
        /// </summary>
        /// <remarks>
        /// This propery should be set at the time the Position is first created and should never change.
        /// </remarks>
        /// <value>
        /// The created by identifier.
        /// </value>
        public int? CreatedById { get; set; }

        /// <summary>
        /// Gets or sets the name of the <see cref="Member"/> that last modified the Position.
        /// </summary>
        /// <remarks>
        /// This property is set to the Display Name of the <see cref="Member"/> who last modified the Position.
        /// </remarks>
        /// <value>
        /// The last modified by.
        /// </value>
        [Display(Name = "Last Modified By")]
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the date that the Position was last modified.
        /// </summary>
        /// <value>
        /// The date the Position was last modified.
        /// </value>
        [Display(Name = "Date Last Modified")]
        public DateTime? LastModified { get; set; }

        /// <summary>
        /// Gets or sets the MemberId of the <see cref="Member"/> that last modified the Position.
        /// </summary>
        /// <value>
        /// The last modified by identifier.
        /// </value>
        public int? LastModifiedById { get; set; }

        
        /// <summary>
        /// Gets or sets the list of all Component Names/Ids in the repository.  Used to populate an HTML select list.
        /// </summary>
        /// <value>
        /// The <see cref="T:List{T}"/> of <see cref="BlueDeck.Types.ComponentSelectListItem"/>s.
        /// </value>
        public List<ComponentSelectListItem> Components { get; set; }

        /// <summary>
        /// Gets or sets a List of current <see cref="Members"/> Members assigned to this Position.
        /// </summary>
        /// <value>
        /// The current members.
        /// </value>
        public List<MemberLineupItem> CurrentMembers { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="MemberSelectListItem"/> that can be assigned to this position
        /// </summary>
        /// <remarks>
        /// As if v1.0, this list is set by the PositionController depending on the Claims of the Current User. 
        /// This list will contain all current Members in the Database if the User is a Global Admin, but will otherwise be limited 
        /// to Members that are stored in the User's claims, which are loaded via the <see cref="ClaimsLoader"/> middleware.
        /// </remarks>
        /// <value>
        /// The available members.
        /// </value>
        public List<MemberSelectListItem> AvailableMembers { get; set; }

        /// TODO: Use a SP to get a list of all Component Names/Ids instead of using EF to pull all Components?
        /// <summary>
        /// Initializes a new instance of the <see cref="PositionWithComponentListViewModel"/> class.
        /// </summary>
        /// <param name="p">The <see cref="BlueDeck.Models.Position"/> being displayed by the view</param>
        public PositionWithComponentListViewModel(Position p) {

            PositionId = p?.PositionId;
            PositionName = p.Name;
            ParentComponentId = p?.ParentComponent?.ComponentId;
            JobTitle = p.JobTitle;
            IsManager = p.IsManager;
            IsAssistantManager = p.IsAssistantManager;
            IsUnique = p.IsUnique;
            LineupPosition = p.LineupPosition;
            CurrentMembers = p.Members.ConvertAll(x => new MemberLineupItem(x));
            if (p.Callsign != "NONE")
            {
                Callsign = p.Callsign;
            }
            Components = new List<ComponentSelectListItem>();
            Creator = p?.Creator?.GetTitleName() ?? "";
            CreatedDate = p?.CreatedDate;
            CreatedById = p?.CreatorId ?? 0;
            LastModifiedBy = p?.LastModifiedBy?.GetTitleName() ?? "";
            LastModified = p?.LastModified;
            LastModifiedById = p?.LastModifiedById ?? 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PositionWithComponentListViewModel"/> class.
        /// <remarks>
        /// This parameter-less constructor had to be added in because the <see cref="PositionWithComponentListViewModel"/> constructor overrode the default, and the form POST model-binding failed
        /// </remarks>
        /// </summary>
        public PositionWithComponentListViewModel() {
            Components = new List<ComponentSelectListItem>();
        }
    }

    /// <summary>
    /// Attribute validator that ensures that a Position is not designated as both Manager and Assistant Manager
    /// </summary>
    /// <seealso cref="System.ComponentModel.DataAnnotations.ValidationAttribute" />
    public class ManageOrAssistantNotBothAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManageOrAssistantNotBothAttribute"/> class.
        /// </summary>
        /// <param name="comparisonProperty">The name of the property to compare.</param>
        public ManageOrAssistantNotBothAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        /// <summary>
        /// Returns Success if both Manager and AssistantManager properties are not true.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>
        /// An instance of the <see cref="System.ComponentModel.DataAnnotations.ValidationResult"></see> class.
        /// </returns>
        /// <exception cref="ArgumentException">Property with this name not found</exception>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            ErrorMessage = ErrorMessageString;
            var currentValue = (bool)value;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (property == null)
                throw new ArgumentException("Property with this name not found");

            var comparisonValue = (bool)property.GetValue(validationContext.ObjectInstance);

            if (currentValue == true && comparisonValue == true)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
