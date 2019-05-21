using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlueDeck.Models.Types;

namespace BlueDeck.Models.ViewModels
{
    public class HomePageViewModel
    {
        public List<HomePageComponentGroup> ComponentGroups { get; set; }
        public Member CurrentUser {get;set;}
        public List<ComponentSelectListItem> Components { get; set; }
        public List<MemberGenderSelectListItem> Genders { get; set; }
        public List<MemberRaceSelectListItem> Races { get; set; }
        public List<MemberRankSelectListItem> Ranks { get; set; }
        public List<HomePageViewModelMemberListItem> ExceptionToDuty { get; set; }

        public HomePageViewModel(Member _member,
            List<ComponentSelectListItem> _components,
            List<MemberGenderSelectListItem> _genders,
            List<MemberRaceSelectListItem> _races,
            List<MemberRankSelectListItem> _ranks)
        {
            CurrentUser = _member;
            ComponentGroups = new List<HomePageComponentGroup>();
            ExceptionToDuty = new List<HomePageViewModelMemberListItem>();
            Components = _components;
            Genders = _genders;
            Races = _races;
            Ranks = _ranks;
        }

        public void GetExceptionToDutyMembers()
        {
            foreach(HomePageComponentGroup g in ComponentGroups)
            {
                foreach (HomePageViewModelMemberListItem m in g.Members)
                {
                    if (m.IsExceptionToNormalDuty == true || m.TempPositionId != null)
                    {
                        ExceptionToDuty.Add(m);
                    }
                }
            }
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
