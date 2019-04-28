using OrgChartDemo.Models.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models.ViewModels
{
    public class AdminComponentIndexListViewModel
    {
        public IEnumerable<AdminComponentIndexViewModelListItem> Components { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string NameSort { get; set; }
        public string ParentComponentNameSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public AdminComponentIndexListViewModel()
        {
            Components = new List<AdminComponentIndexViewModelListItem>();
        }

        public AdminComponentIndexListViewModel(List<Component> _components)
        {
            Components = _components.ConvertAll(x => new AdminComponentIndexViewModelListItem(x));
        }
    }
}
