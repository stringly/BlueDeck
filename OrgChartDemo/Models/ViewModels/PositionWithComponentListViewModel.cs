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
        public string PositionName { get; set; }

        /// <summary>
        /// Gets or sets the Id of the Position's parent <see cref="T:OrgChartDemo.Models.Component"/>
        /// </summary>
        /// <value>
        /// The parent's ComponentId.
        /// </value>
        public int? ParentComponentId { get; set; }

        /// <summary>
        /// Gets or sets the job title.
        /// </summary>
        /// <value>
        /// The job title.
        /// </value>
        [StringLength(75), Required]
        public string JobTitle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this Position is designated as the manager of it's parent <see cref="T:OrgChartDemo.Models.Component"/>.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is manager; otherwise, <c>false</c>.
        /// </value>
        public bool IsManager { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is unique, or if it can be assigned multiple <see cref="T:OrgChartDemo.Models.Member"/>s.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is unique; otherwise, <c>false</c>.
        /// </value>
        public bool IsUnique { get; set; }

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
        /// <param name="l">A <see cref="T:List{T}"/> of all <see cref="T:OrgChartDemo.Models.Component"/>s in the repository </param>
        public PositionWithComponentListViewModel(Position p, List<Component> l) {

            PositionId = p?.PositionId;
            PositionName = p.Name;
            ParentComponentId = p?.ParentComponent?.ComponentId;
            JobTitle = p.JobTitle;
            IsManager = p.IsManager;
            IsUnique = p.IsUnique;
            Components = l.ConvertAll(x => new ComponentSelectListItem { Id = x.ComponentId, ComponentName = x.Name });                  
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:OrgChartDemo.ViewModels.PositionWithComponentListViewModel"/> class.
        /// <remarks>
        /// This parameter-less constructor had to be added in because the <see cref="T:OrgChartDemo.ViewModels.PositionWithComponentListViewModel(Position, List{Component})"/> constructor overrode the default, and the form POST model-binding failed
        /// </remarks>
        /// </summary>
        public PositionWithComponentListViewModel() {

        }
    }
}
