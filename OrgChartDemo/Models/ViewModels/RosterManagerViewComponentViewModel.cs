using Microsoft.AspNetCore.Html;
using OrgChartDemo.Models.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models.ViewModels
{
    public class RosterManagerViewComponentViewModel
    {
        public List<RosterManagerViewModelComponent> ComponentList { get; set; }
       
        public RosterManagerViewComponentViewModel(List<Component> _components)
        {
            ComponentList = _components.ConvertAll(x => new RosterManagerViewModelComponent(x));
            foreach (RosterManagerViewModelComponent c in ComponentList)
            {
                if (c.ParentComponent != null)
                {
                    RosterManagerViewModelComponent parent = ComponentList.Where(x => x.ComponentId == c.ParentComponent.ComponentId).FirstOrDefault();
                    if (parent != null)
                    {
                        parent.Children.Add(c);
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
