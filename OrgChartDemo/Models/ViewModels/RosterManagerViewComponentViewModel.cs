using Microsoft.AspNetCore.Html;
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
                RosterManagerViewModelComponent parent = ComponentList.Where(x => x.ComponentId == c.ParentComponent.ComponentId).FirstOrDefault();
                if (parent != null)
                {
                    parent.Children.Add(c);
                }                
            }
        }
    }
    
    

    public class RosterManagerViewModelComponent : Component
    {
        public List<RosterManagerViewModelComponent> Children { get; set; }

        public RosterManagerViewModelComponent(Component c) : base()
        {
            this.Acronym = c.Acronym;
            this.ComponentId = c.ComponentId;
            this.Name = c.Name;
            this.ParentComponent = c.ParentComponent;
            this.Positions = c.Positions;
            this.Children = new List<RosterManagerViewModelComponent>();
        }
        public HtmlString GetDemographicTableForComponentAndChildren()
        {
            Dictionary<string, int[]> demoTotals = new Dictionary<string, int[]>(){
                {"B", new int[] {0,0}},
                {"W", new int[] {0,0}},
                {"A", new int[] {0,0}},
                {"I", new int[] {0,0}},
                {"U", new int[] {0,0}},
                {"H", new int[] {0,0}}
            };
            
            foreach(Position p in this.Positions)
            {
                foreach(Member m in p.Members)
                {
                    if(m.Gender.GenderId == 2)
                    {
                        demoTotals[m.Race.Abbreviation.ToString()][0]++;
                    }
                    else if (m.Gender.GenderId == 3)
                    {
                        demoTotals[m.Race.Abbreviation.ToString()][1]++;
                    }
                }
            }

            foreach(RosterManagerViewModelComponent c in Children)
            {
                foreach(Position p in c.Positions)
                {
                    foreach(Member m in p.Members)
                    {
                        if(m.Gender.GenderId == 2)
                        {
                            demoTotals[m.Race.Abbreviation.ToString()][0]++;
                        }
                        else if (m.Gender.GenderId == 3)
                        {
                            demoTotals[m.Race.Abbreviation.ToString()][1]++;
                        }
                    }
                }
            }
            return new HtmlString( "<strong>Unit Demographics:</strong><table class='componentDemoTable' data-componentId='" + this.ComponentId + "'>" +
                "<tr>" +
                "<th>Race</th>" +
                "<th>Male</th>" +
                "<th>Female </th>" +
                "</tr>" +
                "<td>Black: </td>" +
                "<td>" + demoTotals["B"][0] + "</td>" +
                "<td>" + demoTotals["B"][1] + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td>White: </td>" +
                "<td> " + demoTotals["W"][0] + " </td>" +
                "<td> " + demoTotals["W"][1] + " </td>" +
                "</tr>" +
                "<tr>" +
                "<td>Asian: </td>" +
                "<td> " + demoTotals["A"][0] + " </td>" +
                "<td> " + demoTotals["A"][1] + " </td>" +
                "</tr>" +
                "<tr>" +
                "<td>American Indian: </td>" +
                "<td> " + demoTotals["I"][0] + " </td>" +
                "<td> " + demoTotals["I"][1] + " </td>" +
                "</tr>" +
                "<tr>" +
                "<td>Hispanic: </td>" +
                "<td> " + demoTotals["H"][0] + " </td>" +
                "<td> " + demoTotals["H"][1] + " </td>" +
                "</tr>" +
                "</table>");
        }
        

    }
}
