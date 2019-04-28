using OrgChartDemo.Models.Types;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrgChartDemo.Models.ViewModels
{
    public class AddPositionToComponentViewComponentViewModel
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

        [Display(Name = "Call Sign")]
        [StringLength(20)]
        public string Callsign { get; set; }

        public int MemberCount { get; set; }

        public Component ParentComponent { get; set; }

        public List<ComponentSelectListItem> ComponentList { get; set; }
        
        public AddPositionToComponentViewComponentViewModel()
        {
        }

        public AddPositionToComponentViewComponentViewModel(Component parent)
        {
            ParentComponentId = parent.ComponentId;
            ParentComponent = parent;
        }

        public AddPositionToComponentViewComponentViewModel(Position position, List<ComponentSelectListItem> componentList)
        {
            ParentComponent = position.ParentComponent;
            ParentComponentId = position.ParentComponent.ComponentId;
            PositionId = position.PositionId;
            PositionName = position.Name;
            JobTitle = position.JobTitle;
            IsManager = position.IsManager;
            IsUnique = position.IsUnique;
            LineupPosition = position.LineupPosition;
            Callsign = position.Callsign;
            ComponentList = componentList;
            MemberCount = position.Members.Count;
        }
    }
}
