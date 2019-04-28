using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
                
        public int ParentComponentId { get; set; }

        /// <summary>
        /// Gets or sets the ParentComponentId.
        /// </summary>
        /// <value>
        /// The ComponentId (PK) of the Parent Component (FK)
        /// </value>
        [Display(Name = "Parent Component")]
        [ForeignKey("ParentComponentId")]
        public virtual Component ParentComponent { get; set; }

        /// <summary>
        /// Gets or sets the Position name.
        /// </summary>
        /// <value>
        /// The name of the Position.
        /// </value>
        [Display(Name = "Position Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Position is unique.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is unique and can be assigned only one Member; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "Unique")]
        public bool IsUnique { get; set; } = false;

        /// <summary>
        /// Gets or sets the Job Title.
        /// </summary>
        /// <value>
        /// The Job Title of a Member assigned to this Position
        /// </value>
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }        

        /// <summary>
        /// Gets or sets a value indicating whether this Position is the manager.
        /// A Component must have exactly one position designated as manager.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is the manager of it's Parent Component.; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "Manager")]
        public bool IsManager { get; set; }

        [Display(Name = "Call Sign")]
        public string Callsign { get; set; }

        /// <summary>
        /// Gets or sets the lineup position.
        /// </summary>
        /// <remarks>
        /// This indicates the current Position's display order among it's Parent Component's Positions
        /// </remarks>
        /// <value>
        /// The lineup position.
        /// </value>
        [Display(Name = "Queue Position")]
        public int? LineupPosition {get;set;}

        public int? CreatorId { get; set; }
        [Display(Name = "Created By")]
        [ForeignKey("CreatorId")]
        public virtual Member Creator { get; set; }
        [Display(Name = "Date Created")]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Date Last Modified")]
        public DateTime LastModified { get; set; }
        public int? LastModifiedById { get; set; }
        [ForeignKey("LastModifiedById")]
        [Display(Name = "Last Modified By")]
        public virtual Member LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the Members.
        /// </summary>
        /// <value>
        /// A collection of Member Entities that represent the members assigned to this Position
        /// </value>
        [Display(Name = "Members")]
        public virtual List<Member> Members { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:OrgChartDemo.Models.Position"/> class.
        /// <remarks>
        /// Parameter-less constructor used to ensure that the <see cref="T:OrgChartDemo.Models.Position.Members"/> <see cref="T:List{T}"/> is initialized.
        /// </remarks>
        /// </summary>
        public Position()
        {
            Members = new List<Member>();
        }      


    }
}
