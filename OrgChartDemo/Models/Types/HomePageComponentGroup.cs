﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models.Types
{
    public class HomePageComponentGroup
    {
        public string ComponentName { get; set; }
        public int ComponentId { get; set; }
        public int? LineupPosition { get; set; }
        public List<HomePageViewModelMemberListItem> Members { get; set; }

        public HomePageComponentGroup(Component c)
        {
            ComponentName = c.Name;
            ComponentId = c.ComponentId;
            LineupPosition = c.LineupPosition;
            Members = new List<HomePageViewModelMemberListItem>();
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