using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models
{
    // this is a base class for creating an Org Chart using GetOrgChart
    // as implemented in this project, these are the minimum fields required

    public class ChartableComponent
    {
        public int id { get; set; }
        public int? parentid { get; set; }
        public string componentName { get; set; }
    }
}
