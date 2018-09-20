using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models {
    /// <summary>
    /// Position Entity
    /// </summary>
    public class Position {
        /// <summary>
        /// Gets or sets the position identifier.
        /// </summary>
        /// <value>
        /// The PositionId PK.
        /// </value>
        [Key]
        public int PositionId { get; set;}
        /// <summary>
        /// Gets or sets the ParentComponentId.
        /// </summary>
        /// <value>
        /// The ComponentId (PK) of the Parent Component (FK)
        /// </value>
        public virtual Component ParentComponent { get; set; }
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
        /// Gets or sets the Members.
        /// </summary>
        /// <value>
        /// A collection of Member Entities that represent the members assigned to this Position
        /// </value>
        public virtual List<Member> Members { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this Position is the manager.
        /// A Component must have exactly one position designated as manager.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is the manager of it's Parent Component.; otherwise, <c>false</c>.
        /// </value>
        public bool IsManager { get; set; }

        public Position()
        {
            Members = new List<Member>();
        }
    }
}
