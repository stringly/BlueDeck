using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrgChartDemo.Models.Types;

namespace OrgChartDemo.Models.ViewModels
{
    public class HomePageViewModel
    {
        public List<HomePageComponentGroup> ComponentGroups { get; set; }
        public Member CurrentUser {get;set;}

        public HomePageViewModel(Member member)
        {
            CurrentUser = member;
            ComponentGroups = new List<HomePageComponentGroup>();

        }

        public void SetComponentOrder(List<HomePageComponentGroup> initial)
        {
            RecurseAndReorderComponents(initial);
               
        }

        public void RecurseAndReorderComponents(List<HomePageComponentGroup> initial)
        {
            HomePageComponentGroup c = initial.First();
            if (ComponentGroups.Contains(c))
            {
                int parentIndex = ComponentGroups.IndexOf(c);
                List<HomePageComponentGroup> children = initial.Where(x => x.ParentComponentId == c.ComponentId)
                                                                        .OrderBy(x => x.LineupPosition)
                                                                        .ToList();
                children.ForEach(x => x.NestedLevel = c.NestedLevel + 1);
                ComponentGroups.InsertRange(parentIndex + 1, children);
                foreach (HomePageComponentGroup child in children)
                {
                    initial.Remove(child);
                }
                if (initial.Count > 0)
                {
                    RecurseAndReorderComponents(initial);
                }
            }
            else
            {
                HomePageComponentGroup parent = ComponentGroups.FirstOrDefault(x => x.ComponentId == c.ParentComponentId);
                if (parent != null)
                {
                    int parentIndex = ComponentGroups.IndexOf(parent);
                    List<HomePageComponentGroup> children = initial.Where(x => x.ParentComponentId == parent.ComponentId)
                                                                            .OrderBy(x => x.LineupPosition)
                                                                            .ToList();
                    children.ForEach(x => x.NestedLevel = parent.NestedLevel + 1);
                    ComponentGroups.InsertRange(parentIndex + 1, children);
                    foreach (HomePageComponentGroup child in children)
                    {
                        initial.Remove(child);
                    }
                    if (initial.Count > 0)
                    {
                        RecurseAndReorderComponents(initial);
                    }
                }
                else
                {
                    c.NestedLevel = 0;
                    ComponentGroups.Add(c);
                    initial.Remove(c);
                    if (initial.Count > 0)
                    {
                        RecurseAndReorderComponents(initial);
                    }
                }
            }

        }
    }
}
