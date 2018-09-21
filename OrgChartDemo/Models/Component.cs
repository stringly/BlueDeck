using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models {
    /// <summary>
    /// Component Entity
    /// </summary>
    public class Component {
        /// <summary>
        /// Gets or sets the Component's Id (PK).
        /// </summary>
        /// <value>
        /// The Component's (PK) identifier.
        /// </value>
        [Key]
        public int ComponentId { get; set; }

        /// <summary>
        /// Gets or sets the component's parent <see cref="T:OrgChartDemo.Models.Component"/>.
        /// </summary>
        /// <value>
        /// The Component's parent <see cref="T:OrgChartDemo.Models.Component"/>
        /// </value>
        public virtual Component ParentComponent { get; set; }

        /// <summary>
        /// Gets or sets the Component's name.
        /// </summary>
        /// <value>
        /// The name of the Component.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Component's acronym.
        /// </summary>
        /// <value>
        /// The acronym of the Component
        /// </value>
        public string Acronym { get; set; }

        /// <summary>
        /// Gets or sets the list of the <see cref="T:OrgChartDemo.Models.Position"/>s assinged to this Component.
        /// </summary>
        /// <value>
        /// An <see cref="T:ICollection{T}"/> of <see cref="T:OrgChartDemo.Models.Position"/>s.
        /// </value>
        public virtual ICollection<Position> Positions { get; set; }
    }
}
