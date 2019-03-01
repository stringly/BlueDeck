using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Display(Name = "Parent Component")]
        public virtual Component ParentComponent { get; set; }

        /// <summary>
        /// Gets or sets the Component's name.
        /// </summary>
        /// <value>
        /// The name of the Component.
        /// </value>
        [Display(Name = "Component Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Component's acronym.
        /// </summary>
        /// <value>
        /// The acronym of the Component
        /// </value>        
        [Display(Name = "Acronym")]
        public string Acronym { get; set; }

        /// <summary>
        /// Gets or sets the lineup position.
        /// </summary>
        /// <remarks>
        /// This property is used to control the order in which the component is displayed among it's sibling components
        /// </remarks>
        /// <value>
        /// The lineup position.
        /// </value>
        public int? LineupPosition { get; set; }

        /// <summary>
        /// Gets or sets the list of the <see cref="T:OrgChartDemo.Models.Position"/>s assinged to this Component.
        /// </summary>
        /// <value>
        /// An <see cref="T:ICollection{T}"/> of <see cref="T:OrgChartDemo.Models.Position"/>s.
        /// </value>
        public ICollection<Position> Positions { get; set; }
        
        public virtual ICollection<Component> ChildComponents { get; set; }

        public Component()
        {            
        }


        public int GetComponentMemberGenderCountFemale()
        {
            int totalCount = 0;
            if (Positions != null)
            {
                foreach(Position p in Positions)
                {   
                    if (p.Members != null)
                    {
                        foreach (Member m in p.Members)
                        {
                            if (m.Gender.GenderFullName == "Female")
                            {
                                totalCount++;
                            }
                        }
                    }
                }
            }
            return totalCount;
        }

        public int GetComponentMemberGenderCountMale()
        {
            int totalCount = 0;
            if (Positions != null)
            {
                foreach(Position p in Positions)
                {   
                    if (p.Members != null)
                    {
                        foreach (Member m in p.Members)
                        {
                            if (m.Gender.GenderFullName == "Male")
                            {
                                totalCount++;
                            }
                        }
                    }
                }
            }
            return totalCount;
        }
    }
}
