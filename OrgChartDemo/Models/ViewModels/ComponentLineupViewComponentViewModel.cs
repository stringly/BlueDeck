using System.Collections.Generic;
using System.Linq;
using OrgChartDemo.Models.Types;

namespace OrgChartDemo.Models.ViewModels
{
    public class ComponentLineupViewComponentViewModel
    {
        public List<ComponentPositionLineupItem> Components { get; set; }
        public Component ComponentToEdit { get; set; }
        public ComponentLineupViewComponentViewModel(List<ComponentPositionLineupItem> components, Component componentBeingEdited = null)
        {
            Components = components.OrderBy(x => x.LineupPosition).ToList();
            if (componentBeingEdited == null)
            {
                ComponentToEdit = new Component();
            }
            else
            {
                ComponentToEdit = componentBeingEdited;
            }
        }
    }
}
