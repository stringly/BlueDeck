using System.Collections.Generic;
using BlueDeck.Models.Types;
using System.ComponentModel.DataAnnotations;

namespace BlueDeck.Models.ViewModels
{
    /// <summary>
    /// ViewModel used to display a Component and populate a selectlist of Component Names/Ids to facilitate adding a Component or changing the Component to which a Component is assigned. 
    /// </summary>
    public class ComponentWithComponentListViewModel
    {
        /// <summary>
        /// Gets or sets the Id of the Component.
        /// </summary>
        /// <value>
        /// The component id.
        /// </value>
        public int? ComponentId { get; set; }

        /// <summary>
        /// Gets or sets the Component Name.
        /// </summary>
        /// <value>
        /// The name of the component.
        /// </value>
        [StringLength(75), Required]        
        [Display(Name = "Component Name")]
        public string ComponentName { get; set; }

        /// <summary>
        /// Gets or sets the Id of the Component's Parent Component
        /// </summary>
        /// <value>
        /// The Id of the Component's Parent Component.
        /// </value>
        [Required]
        [Display(Name = "Parent Component")]
        public int? ParentComponentId { get; set; }

        /// <summary>
        /// Gets or sets the Component's acronym.
        /// </summary>
        /// <value>
        /// The acronym of the Component.
        /// </value>
        [Display(Name = "Acronym")]
        [StringLength(10)]
        public string Acronym { get; set; }

        public int? LineupPosition { get; set; }
        [Display(Name = "Created By")]
        public string Creator { get; set; }
        [Display(Name = "Date Created")]
        public System.DateTime? CreatedDate { get; set; }
        [Display(Name = "Last Modified By")]
        public string LastModifiedBy { get; set; }
        [Display(Name = "Date Last Modified")]
        public System.DateTime? LastModified { get; set; }
        public int? LastModifiedById { get; set; }
        public int? CreatedById { get; set; }
        /// <summary>
        /// Gets or sets the list of all Component Names/Ids in the repository.  Used to populate an HTML select list.
        /// </summary>
        /// <value>
        /// The <see cref="T:List{T}"/> of <see cref="T:BlueDeck.Types.ComponentSelectListItem"/>s.
        /// </value>
        public List<ComponentSelectListItem> Components { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:BlueDeck.ViewModels.ComponentWithComponentListViewModel"/> class.
        /// <remarks>
        /// This parameter-less constructor had to be added in because the <see cref="T:BlueDeck.ViewModels.ComponentWithComponentListViewModel(Component, List{Component})"/> constructor overrode the default, and the form POST model-binding failed
        /// </remarks>
        /// </summary>
        public ComponentWithComponentListViewModel()
        {
        }

        /// TODO: Use a SP to get a list of all Component Names/Ids instead of using EF to pull all Components?
        /// <summary>
        /// Initializes a new instance of the <see cref="T:BlueDeck.ViewModels.ComponentWithComponentListViewModel"/> class.
        /// </summary>
        /// <param name="c">The <see cref="T:BlueDeck.Models.Component"/> being displayed by the view</param>
        /// <param name="l">A <see cref="T:List{T}"/> of all <see cref="T:BlueDeck.Models.Component"/>s in the repository </param>
        public ComponentWithComponentListViewModel(Component c, List<ComponentSelectListItem> l)
        {
            ComponentId = c?.ComponentId;
            ComponentName = c.Name;
            ParentComponentId = c?.ParentComponent?.ComponentId;
            Acronym = c.Acronym;
            LineupPosition = c.LineupPosition;
            Components = l;
            Creator = c?.Creator?.GetTitleName() ?? "";
            CreatedDate = c?.CreatedDate;
            CreatedById = c?.CreatorId ?? 0;
            LastModifiedBy = c?.LastModifiedBy?.GetTitleName() ?? "";
            LastModified = c?.LastModified;
            LastModifiedById = c?.LastModifiedById ?? 0;

        }
    }
}
