using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models {

    // extends Chartable to add Member fields to pass to a view
    // using GetOrgChart as implemented in this project

    public class ChartableComponentWithManager : ChartableComponent {

        public int? memberId { get; set; }
        public string memberName { get; set; }
        public string email { get; set; }
        public string contactNumber { get; set; }        
    }
}
