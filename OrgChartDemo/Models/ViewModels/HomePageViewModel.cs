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

        public void SetComponentGroups(List<Component> _components)
        {
            List<HomePageComponentGroup> initial = _components.ConvertAll(x => new HomePageComponentGroup(x));
            SetComponentOrder(initial);

        }

        public void SetComponentOrder(List<HomePageComponentGroup> _initial)
        {
            HomePageComponentGroup currentGroup = _initial.First();
            if (ComponentGroups.Count == 0)
            {
                // no components, so assume we are on the first.
                currentGroup.NestedLevel = 0;
                ComponentGroups.Add(currentGroup);

                List<HomePageComponentGroup> children = _initial.Where(x => x.ParentComponentId == currentGroup.ComponentId).ToList();
                if (children != null)
                {
                    children.ForEach(x => x.NestedLevel = currentGroup.NestedLevel + 1);
                    ComponentGroups.AddRange(children);
                }               

                _initial.Remove(currentGroup);
                foreach(HomePageComponentGroup child in children)
                {
                    _initial.Remove(child);
                }
                if (_initial.Count > 0)
                {
                    SetComponentOrder(_initial);
                }
            }
            else
            {
                // there are already groups, so we need to see if current component is a child
                HomePageComponentGroup parent = ComponentGroups.Where(x => x.ComponentId == currentGroup.ParentComponentId).FirstOrDefault();               
                if (parent != null)
                {
                    int insertIndex = ComponentGroups.IndexOf(parent) + 1;
                    currentGroup.NestedLevel = parent.NestedLevel + 1;
                    ComponentGroups.Insert(insertIndex, currentGroup);
                    List<HomePageComponentGroup> children = _initial.Where(x => x.ParentComponentId == currentGroup.ComponentId).ToList();
                    if (children != null)
                    {
                        children.ForEach(x => x.NestedLevel = currentGroup.NestedLevel + 1);
                        ComponentGroups.InsertRange(insertIndex +1, children);
                    }
                    _initial.Remove(currentGroup);
                    foreach(HomePageComponentGroup child in children)
                    {
                        _initial.Remove(child);
                    }
                    if (_initial.Count > 0)
                    {
                        SetComponentOrder(_initial);
                    }
                }
                else
                {
                    // Component has no parent
                    currentGroup.NestedLevel = 0;
                    ComponentGroups.Add(currentGroup);
                    List<HomePageComponentGroup> children = _initial.Where(x => x.ParentComponentId == currentGroup.ComponentId).ToList();
                    if (children != null)
                    {
                        children.ForEach(x => x.NestedLevel = currentGroup.NestedLevel + 1);
                        ComponentGroups.AddRange(children);
                    }
                    _initial.Remove(currentGroup);
                    foreach(HomePageComponentGroup child in children)
                    {
                        _initial.Remove(child);
                    }
                    if (_initial.Count > 0)
                    {
                        SetComponentOrder(_initial);
                    }
                }
            }
        }

        
    }
}
