using Microsoft.AspNetCore.Html;
using BlueDeck.Models.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.ViewModels
{
    public class RosterManagerViewComponentViewModel
    {
        public List<RosterManagerViewModelComponent> ComponentList { get; set; }
       
        public RosterManagerViewComponentViewModel(List<Component> _components)
        {
            List<RosterManagerViewModelComponent> initial = _components.ConvertAll(x => new RosterManagerViewModelComponent(x));

            foreach (RosterManagerViewModelComponent c in initial)
            {
                if (c.ParentComponent != null)
                {
                    RosterManagerViewModelComponent parent = initial.Where(x => x.ComponentId == c.ParentComponent.ComponentId).FirstOrDefault();
                    if (parent != null)
                    {
                        parent.Children.Add(c);
                    }
                }
            }
            ComponentList = new List<RosterManagerViewModelComponent>();

            // this is such a hack. Earlier iterations, the Components on the Component List were likely to be in order already, so the Parent would get evaluated first.
            // I realized this was a problem when this ran for a Parent Component with a ComponentId HIGHER than the ComponentId of one of it's children.
            // To hack around this, I manually extract and push the "root" component (which means the Component on the "initial" list that has a NULL ParentComponent)
            // to the ComponentList Property and then remove it from the "intial" list prior to calling the SetComponentListOrder method.
            RosterManagerViewModelComponent rootComponent = initial.Where(x => x.ParentComponent == null).First();
            rootComponent.NestedLevel = 0;
            ComponentList.Add(rootComponent);
            initial.Remove(rootComponent);
            if(initial.Count() > 0)
            {
                SetComponentListOrder(initial.OrderBy(x => x.ParentComponentId).ToList());
            }           

        }

        private void SetComponentListOrder(List<RosterManagerViewModelComponent> initial)
        {
            RosterManagerViewModelComponent c = initial.First();
            if (ComponentList.Contains(c))
            {
                int parentIndex = ComponentList.IndexOf(c);
                List<RosterManagerViewModelComponent> children = initial.Where(x => x.ParentComponent.ComponentId == c.ComponentId)
                                                                        .OrderBy(x => x.LineupPosition)
                                                                        .ToList();
                children.ForEach(x => x.NestedLevel = c.NestedLevel + 1);
                ComponentList.InsertRange(parentIndex + 1, children);
                foreach (RosterManagerViewModelComponent child in children)
                {
                    initial.Remove(child);
                }
                if (initial.Count > 0)
                {
                    SetComponentListOrder(initial);
                }                
            }
            else
            {
                RosterManagerViewModelComponent parent = ComponentList.FirstOrDefault(x => x.ComponentId == c.ParentComponent.ComponentId);
                if (parent != null)
                {
                    int parentIndex = ComponentList.IndexOf(parent);
                    List<RosterManagerViewModelComponent> children = initial.Where(x => x.ParentComponent.ComponentId == parent.ComponentId)
                                                                            .OrderBy(x => x.LineupPosition)
                                                                            .ToList();
                    children.ForEach(x => x.NestedLevel = parent.NestedLevel + 1);
                    ComponentList.InsertRange(parentIndex + 1, children);
                    foreach (RosterManagerViewModelComponent child in children)
                    {
                        initial.Remove(child);
                    }
                    if (initial.Count > 0)
                    {
                        SetComponentListOrder(initial);
                    }
                }
                else
                {
                    c.NestedLevel = 0;
                    ComponentList.Add(c);
                    initial.Remove(c);
                    if (initial.Count > 0)
                    {
                        SetComponentListOrder(initial);
                    }
                }
            }
        }

        public Dictionary<string, string> GetDemoTableDictionaryForAllComponents()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach(RosterManagerViewModelComponent c in ComponentList)
            {
                result.Add("#demographicsgroup_" + c.ComponentId, c.GetDemographicTableForComponentAndChildren().ToString());
            }
            return result;
        }
    }

}
