using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BlueDeck.Models.Types;

namespace BlueDeck.Models.ViewModels
{
    public class ComponentLineupViewComponentViewModel
    {
        [Display(Name = "Display Order")]
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
