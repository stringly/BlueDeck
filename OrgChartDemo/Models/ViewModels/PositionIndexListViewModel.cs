using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using OrgChartDemo.Models.Types;

namespace OrgChartDemo.Models.ViewModels
{
    public class PositionIndexListViewModel
    {
        public IEnumerable<PositionIndexViewModelPositionListItem> Positions { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string PositionNameSort { get; set; }
        public string ParentComponentNameSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public PositionIndexListViewModel(List<Position> positions)
        {
            Positions = positions.ConvertAll(x => new PositionIndexViewModelPositionListItem(x));
        }
    }
    
}
