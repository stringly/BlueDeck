using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models
{

    /// <summary>
    /// Base class for creating an Organization Chart using GetOrgChart
    /// </summary>
    public class ChartableComponent
    {
        public int id { get; set; }
        public int? parentid { get; set; }
        public string componentName { get; set; }
    }
}
