using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BlueDeck.Models.Types
{
    public class ComponentIndexViewModelComponentListItem
    {
        public int ComponentId { get; set; }
        [Display(Name = "Name")]
        public string ComponentName { get; set; }
        public int ParentComponentId { get; set; }
        [Display(Name = "Parent Component")]
        public string ParentComponentName { get; set; }
        public string Acronym { get; set; }
        [Display(Name = "Positions")]
        public int PositionCount { get; set; }
        [Display(Name = "Members")]
        public int MemberCount { get; set; }

        public ComponentIndexViewModelComponentListItem()
        {
        }
        public ComponentIndexViewModelComponentListItem(Component c)
        {            
            ComponentId = c.ComponentId;
            ComponentName = c.Name;
            ParentComponentId = c?.ParentComponent?.ComponentId ?? 0;
            ParentComponentName = c?.ParentComponent?.Name ?? "Police Department";
            Acronym = c?.Acronym ?? "-";
            PositionCount = c.Positions.Count();

            int totalMembers = 0;
            if (c.Positions.Count > 0)
            {
                
                foreach (Position p in c.Positions)
                {
                    totalMembers += p.Members.Count();
                }
            }
            MemberCount = totalMembers;
        }
    }
}
