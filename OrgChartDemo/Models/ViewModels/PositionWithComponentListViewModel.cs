using System.Collections.Generic;
using OrgChartDemo.Models.Types;
using System.ComponentModel.DataAnnotations;

namespace OrgChartDemo.Models.ViewModels
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
        /// Gets or sets the Id of the Position's parent <see cref="T:OrgChartDemo.Models.Component"/>
        /// </summary>
        /// <value>
        /// The parent's ComponentId.
        /// </value>
        [Display(Name = "Parent Component")] 
        public int? ParentComponentId { get; set; }

        /// <summary>
        /// Gets or sets the job title.
        /// </summary>
        /// <value>
        /// The job title.
        /// </value>
        [Required]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this Position is designated as the manager of it's parent <see cref="T:OrgChartDemo.Models.Component"/>.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is manager; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "Manager of Component")] 
        public bool IsManager { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is unique, or if it can be assigned multiple <see cref="T:OrgChartDemo.Models.Member"/>s.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is unique; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "Unique")]
        public bool IsUnique { get; set; }

        public int? LineupPosition { get; set; }

        /// <summary>
        /// Gets or sets the list of all Component Names/Ids in the repository.  Used to populate an HTML select list.
        /// </summary>
        /// <value>
        /// The <see cref="T:List{T}"/> of <see cref="T:OrgChartDemo.Types.ComponentSelectListItem"/>s.
        /// </value>
        public List<ComponentSelectListItem> Components { get; set; }


        /// TODO: Use a SP to get a list of all Component Names/Ids instead of using EF to pull all Components?
        /// <summary>
        /// Initializes a new instance of the <see cref="T:OrgChartDemo.ViewModels.PositionWithComponentListViewModel"/> class.
        /// </summary>
        /// <param name="p">The <see cref="T:OrgChartDemo.Models.Position"/> being displayed by the view</param>
        public PositionWithComponentListViewModel(Position p) {

            PositionId = p?.PositionId;
            PositionName = p.Name;
            ParentComponentId = p?.ParentComponent?.ComponentId;
            JobTitle = p.JobTitle;
            IsManager = p.IsManager;
            IsUnique = p.IsUnique;
            LineupPosition = p.LineupPosition;
            Components = new List<ComponentSelectListItem>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:OrgChartDemo.ViewModels.PositionWithComponentListViewModel"/> class.
        /// <remarks>
        /// This parameter-less constructor had to be added in because the <see cref="T:OrgChartDemo.ViewModels.PositionWithComponentListViewModel(Position, List{Component})"/> constructor overrode the default, and the form POST model-binding failed
        /// </remarks>
        /// </summary>
        public PositionWithComponentListViewModel() {
            Components = new List<ComponentSelectListItem>();
        }
    }
}
