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
        public List<HomePageViewModelMemberListItem> TempMembers { get; set; }
        public string StrengthDisplay { get; set; }
        
        public HomePageComponentGroup(Component c)
        {
            ComponentName = c.Name;
            ComponentId = c.ComponentId;
            LineupPosition = c.LineupPosition;
            ParentComponentId = c.ParentComponentId;
            Members = new List<HomePageViewModelMemberListItem>();
            TempMembers = new List<HomePageViewModelMemberListItem>();
            int managerCount = 0;
            int workerCount = 0;
            if (c.Positions != null)
            {
                foreach (Position p in c.Positions.OrderBy(x => x.LineupPosition))
                {                    
                    // if there are no primary or TDY members, render a "Vacant" item by invoking the 
                    // constructor that takes a position as a parameter                    
                    if ((p.Members == null || p.Members.Count == 0) && (p.TempMembers == null || p.TempMembers.Count == 0))
                    {
                        HomePageViewModelMemberListItem mi = new HomePageViewModelMemberListItem(p);
                        Members.Add(mi);
                    }
                    else
                    {
                        if (p.Members != null)
                        {
                            foreach (Member m in p.Members)
                            {
                                HomePageViewModelMemberListItem mi = new HomePageViewModelMemberListItem(m);
                                Members.Add(mi);
                            }
                        }
                        if (p.TempMembers != null)
                        {
                            foreach (Member m in p.TempMembers)
                            {
                                HomePageViewModelMemberListItem mi = new HomePageViewModelMemberListItem(m);
                                TempMembers.Add(mi);
                            }
                        }                        
                    }
                }
            }
            StrengthDisplay = $"{c.GetManagerCount()} and {c.GetWorkerCount()}";
        }        
    }
}
