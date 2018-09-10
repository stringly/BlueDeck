using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models {

    public class OrgChartComponentWithMember {

        public int id { get; set; }
        public int? parentid { get; set; }
        public string componentName { get; set; }        
        public int? memberId { get; set; }
        public string memberName { get; set; }
        public string email { get; set; }
        public string contactNumber { get; set; }
        
    }
}
