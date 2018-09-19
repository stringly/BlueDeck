using System.Collections.Generic;
using OrgChartDemo.Models.Types;
using System.ComponentModel.DataAnnotations;

namespace OrgChartDemo.Models.ViewModels
{
    public class PositionWithComponentListViewModel
    {
        public int? PositionId { get; set; }
        [StringLength(75), Required]
        public string PositionName { get; set; }
        public int? ParentComponentId { get; set; }
        [StringLength(75), Required]
        public string JobTitle { get; set; }
        public bool IsManager { get; set; }
        public bool IsUnique { get; set; }
        public List<ComponentSelectListItem> Components { get; set; }

        

        public PositionWithComponentListViewModel(Position p, List<Component> l) {

            PositionId = p?.PositionId;
            PositionName = p.Name;
            ParentComponentId = p?.ParentComponent?.ComponentId;
            JobTitle = p.JobTitle;
            IsManager = p.IsManager;
            IsUnique = p.IsUnique;
            Components = l.ConvertAll(x => new ComponentSelectListItem { Id = x.ComponentId, ComponentName = x.Name });                  
        }

        public PositionWithComponentListViewModel() {

        }
    }
}
