using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models.ViewModels
{
    public class OrgChartOptionsViewModel
    {
        public List<Component> Components { get; set; }
        public List<Member> Members { get; set; }
    }
}
