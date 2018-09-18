using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models {
    public class Component {
        [Key]
        public int ComponentId { get; set; }
        public virtual Component ParentComponent { get; set; }
        public string Name { get; set; }
        public string Acronym { get; set; }
        public virtual ICollection<Position> Positions { get; set; }
    }
}
