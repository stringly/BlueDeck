using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models.ViewModels
{
    public class PositionWithComponentListViewModel
    {
        public Position Position { get; set; }
        public List<Component> Components { get; set; }
    }
}
