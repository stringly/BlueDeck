using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.Types
{
    public class AdminComponentIndexViewModelListItem
    {
        public int ComponentId { get; set; }
        public string ComponentName { get; set; }
        public int? ParentComponentId { get; set; }
        public string ParentComponentName { get; set; }
        public string Acronym { get; set; }
        public int? LineupPosition { get; set; }
        public int ChildrenCount { get; set; }
        public string ManagerName { get; set; }

        public AdminComponentIndexViewModelListItem()
        {
        }

        public AdminComponentIndexViewModelListItem(Component _component)
        {
            ComponentId = _component.ComponentId;
            ComponentName = _component.Name;
            Acronym = _component.Acronym;
            LineupPosition = _component.LineupPosition;
            ChildrenCount = _component.ChildComponents.Count();
            ManagerName = _component?.Positions?.FirstOrDefault(x => x.IsManager == true)?.Members?.FirstOrDefault()?.GetTitleName() ?? "None";
            ParentComponentId = _component?.ParentComponentId ?? 0;
            ParentComponentName = _component?.ParentComponent?.Name ?? "None";

        }
    }
}
