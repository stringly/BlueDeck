using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.Types
{
    public class HomePageComponentGroup
    {
        public string ComponentName { get; set; }
        public int ComponentId { get; set; }
        public int? LineupPosition { get; set; }
        public int? ParentComponentId { get; set; }
        public int NestedLevel {get;set;}
        public List<HomePageViewModelMemberListItem> Members { get; set; }

        public HomePageComponentGroup(Component c)
        {
            ComponentName = c.Name;
            ComponentId = c.ComponentId;
            LineupPosition = c.LineupPosition;
            ParentComponentId = c.ParentComponentId;
            Members = new List<HomePageViewModelMemberListItem>();
            if (c.Positions != null)
            {
                foreach (Position p in c.Positions.OrderBy(x => x.LineupPosition))
                {
                    if (p.Members.Count > 0)
                    {
                        foreach (Member m in p.Members)
                        {
                            HomePageViewModelMemberListItem mi = new HomePageViewModelMemberListItem(m);
                            Members.Add(mi);
                        }
                    }
                    else
                    {
                        HomePageViewModelMemberListItem mi = new HomePageViewModelMemberListItem(p);
                        Members.Add(mi);
                    }
                }
            }
            
        }
    }
}
