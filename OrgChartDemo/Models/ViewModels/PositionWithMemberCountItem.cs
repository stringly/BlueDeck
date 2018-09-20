using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models.ViewModels
{
    /// <summary>
    /// A flattened object derived from a Position Entity and used in <see cref="OrgChartDemo.Controllers.PositionsController.Index"/>/>
    /// </summary>
    public class PositionWithMemberCountItem
    {
        /// <summary>
        /// Gets or sets the position identifier.
        /// </summary>
        /// <value>
        /// The PositionId PK.
        /// </value>
        public int PositionId { get; set;}
        /// <summary>
        /// Gets or sets the id of the parent component.
        /// </summary>
        /// <value>
        /// The Id of the Component to which this Position Instance is assigned
        /// </value>
        public int? ParentComponentId { get; set; }
        /// <summary>
        /// Gets or sets the name of the parent component.
        /// </summary>
        /// <value>
        /// The name of the parent component.
        /// </value>
        public string ParentComponentName { get; set; }        
        /// <summary>
        /// Gets or sets the Position name.
        /// </summary>
        /// <value>
        /// The name of the Position.
        /// </value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the Position is unique.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is unique and can be assigned only one Member; otherwise, <c>false</c>.
        /// </value>
        public bool IsUnique { get; set; } = false;
        /// <summary>
        /// Gets or sets the Job Title.
        /// </summary>
        /// <value>
        /// The Job Title of a Member assigned to this Position
        /// </value>
        public string JobTitle { get; set; }
        /// <summary>
        /// Gets or sets the count of Position Members
        /// </summary>
        /// <value>
        /// A count of Member Entities that are assigned to this Position
        /// </value>
        public int MembersCount { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this Position is the manager.
        /// A Component must have exactly one position designated as manager.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is the manager of it's Parent Component.; otherwise, <c>false</c>.
        /// </value>
        public bool IsManager { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="PositionWithMemberCountItem"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor requires a <see cref="Position"/> to be passed as a parameter.
        /// </remarks>
        /// <param name="p">A <see cref="Position"/> object.</param>
        public PositionWithMemberCountItem(Position p) {
            PositionId = p.PositionId;
            ParentComponentId = p.ParentComponent.ComponentId;
            ParentComponentName = p.ParentComponent.Name;
            Name = p.Name;
            IsUnique = p.IsUnique;
            IsManager = p.IsManager;
            JobTitle = p.JobTitle;
            MembersCount = p?.Members?.Count() ?? 0;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PositionWithMemberCountItem"/>.
        /// <remarks>
        /// This is a parameterless constructor used to create new objects
        /// </remarks>
        /// </summary>
        public PositionWithMemberCountItem()
        {

        }
    }
}
