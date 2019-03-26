using OrgChartDemo.Models.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models.ViewModels
{
    public class OrgChartIndexViewModel
    {
        public int SelectedComponentId { get; set; }
        public List<ComponentSelectListItem> ComponentList { get; set; }

        public OrgChartIndexViewModel()
        {
            ComponentList = new List<ComponentSelectListItem>();
        }
    }
}
