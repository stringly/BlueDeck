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
            List<GenderRankDemoCollectionObject> rankGenders = new List<GenderRankDemoCollectionObject>()
            {
                {new GenderRankDemoCollectionObject("P/O")},
                {new GenderRankDemoCollectionObject("POFC")},
                {new GenderRankDemoCollectionObject("Cpl.")},
                {new GenderRankDemoCollectionObject("Sgt.")},
                {new GenderRankDemoCollectionObject("Lt.")},
                {new GenderRankDemoCollectionObject("Capt.")},
                {new GenderRankDemoCollectionObject("Exec")}
            };

            Dictionary<string, BundledGenderRankDemoCollectionObject> demoEmpty = new Dictionary<string, BundledGenderRankDemoCollectionObject>(){
                {"B", new BundledGenderRankDemoCollectionObject()},
                {"W", new BundledGenderRankDemoCollectionObject()},
                {"A", new BundledGenderRankDemoCollectionObject()},
                {"I", new BundledGenderRankDemoCollectionObject()},
                {"U", new BundledGenderRankDemoCollectionObject()},
                {"H", new BundledGenderRankDemoCollectionObject()}
            };
            Dictionary<string, List<GenderRankDemoCollectionObject>> demoTotals = RecurseForChildComponentDemoTotals(this, demoEmpty);
            
            return new HtmlString( "<strong>Unit Demographics:</strong><table class='table componentDemoTable' data-componentId='" + this.ComponentId + "'>" +
                "<tr>" +
                    "<th>" +
                    "<th>P/O" +
                    "<th>POFC" +
                    "<th>Cpl." +
                    "<th>Sgt." +
                    "<th>Lt." +
                    "<th>Capt" +
                    "<th>Exec" +
                "</tr>" +
                "<tr>" +
                    "<td>Race</td>" +
                    "<td>M/F</td>" +
                    "<td>M/F</td>" +
                    "<td>M/F</td>" +
                    "<td>M/F</td>" +
                    "<td>M/F</td>" +
                    "<td>M/F</td>" +
                    "<td>M/F</td>" +
                "</tr>" +                    
                    "<td>Black: </td>" +
                    "<td>" + demoTotals["B"].First(x => x.RankName == "P/O").GenderCount["Male"] + "/" + demoTotals["B"].First(x => x.RankName == "P/O").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals["B"].First(x => x.RankName == "POFC").GenderCount["Male"] + "/" + demoTotals["B"].First(x => x.RankName == "POFC").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals["B"].First(x => x.RankName == "Cpl.").GenderCount["Male"] + "/" + demoTotals["B"].First(x => x.RankName == "Cpl.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals["B"].First(x => x.RankName == "Sgt.").GenderCount["Male"] + "/" + demoTotals["B"].First(x => x.RankName == "Sgt.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals["B"].First(x => x.RankName == "Lt.").GenderCount["Male"] + "/" + demoTotals["B"].First(x => x.RankName == "Lt.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals["B"].First(x => x.RankName == "Capt.").GenderCount["Male"] + "/" + demoTotals["B"].First(x => x.RankName == "Capt.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals["B"].First(x => x.RankName == "Exec").GenderCount["Male"] + "/" + demoTotals["B"].First(x => x.RankName == "Exec").GenderCount["Female"] + "</td>" +
                "</tr>" +
                "<tr>" +
                    "<td>White: </td>" +
                    "<td>" + demoTotals["W"].First(x => x.RankName == "P/O").GenderCount["Male"] + "/" + demoTotals["W"].First(x => x.RankName == "P/O").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals["W"].First(x => x.RankName == "POFC").GenderCount["Male"] + "/" + demoTotals["W"].First(x => x.RankName == "POFC").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals["W"].First(x => x.RankName == "Cpl.").GenderCount["Male"] + "/" + demoTotals["W"].First(x => x.RankName == "Cpl.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals["W"].First(x => x.RankName == "Sgt.").GenderCount["Male"] + "/" + demoTotals["W"].First(x => x.RankName == "Sgt.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals["W"].First(x => x.RankName == "Lt.").GenderCount["Male"] + "/" + demoTotals["W"].First(x => x.RankName == "Lt.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals["W"].First(x => x.RankName == "Capt.").GenderCount["Male"] + "/" + demoTotals["W"].First(x => x.RankName == "Capt.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals["W"].First(x => x.RankName == "Exec").GenderCount["Male"] + "/" + demoTotals["W"].First(x => x.RankName == "Exec").GenderCount["Female"] + "</td>" +
                "</tr>" +
                "<tr>" +
                    "<td>Asian: </td>" +
                    "<td>" + demoTotals["A"].First(x => x.RankName == "P/O").GenderCount["Male"] + "/" + demoTotals["A"].First(x => x.RankName == "P/O").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals["A"].First(x => x.RankName == "POFC").GenderCount["Male"] + "/" + demoTotals["A"].First(x => x.RankName == "POFC").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals["A"].First(x => x.RankName == "Cpl.").GenderCount["Male"] + "/" + demoTotals["A"].First(x => x.RankName == "Cpl.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals["A"].First(x => x.RankName == "Sgt.").GenderCount["Male"] + "/" + demoTotals["A"].First(x => x.RankName == "Sgt.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals["A"].First(x => x.RankName == "Lt.").GenderCount["Male"] + "/" + demoTotals["A"].First(x => x.RankName == "Lt.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals["A"].First(x => x.RankName == "Capt.").GenderCount["Male"] + "/" + demoTotals["A"].First(x => x.RankName == "Capt.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals["A"].First(x => x.RankName == "Exec").GenderCount["Male"] + "/" + demoTotals["A"].First(x => x.RankName == "Exec").GenderCount["Female"] + "</td>" +
                "</tr>" +
                "<tr>" +
                    "<td>Am/Indian: </td>" +
                    "<td>" + demoTotals["I"].First(x => x.RankName == "P/O").GenderCount["Male"] + "/" + demoTotals["I"].First(x => x.RankName == "P/O").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals["I"].First(x => x.RankName == "POFC").GenderCount["Male"] + "/" + demoTotals["I"].First(x => x.RankName == "POFC").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals["I"].First(x => x.RankName == "Cpl.").GenderCount["Male"] + "/" + demoTotals["I"].First(x => x.RankName == "Cpl.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals["I"].First(x => x.RankName == "Sgt.").GenderCount["Male"] + "/" + demoTotals["I"].First(x => x.RankName == "Sgt.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals["I"].First(x => x.RankName == "Lt.").GenderCount["Male"] + "/" + demoTotals["I"].First(x => x.RankName == "Lt.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals["I"].First(x => x.RankName == "Capt.").GenderCount["Male"] + "/" + demoTotals["I"].First(x => x.RankName == "Capt.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals["I"].First(x => x.RankName == "Exec").GenderCount["Male"] + "/" + demoTotals["I"].First(x => x.RankName == "Exec").GenderCount["Female"] + "</td>" +
                "</tr>" +
                "<tr>" +
                    "<td>Hispanic: </td>" +
                    "<td>" + demoTotals["H"].First(x => x.RankName == "P/O").GenderCount["Male"] + "/" + demoTotals["H"].First(x => x.RankName == "P/O").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals["H"].First(x => x.RankName == "POFC").GenderCount["Male"] + "/" + demoTotals["H"].First(x => x.RankName == "POFC").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals["H"].First(x => x.RankName == "Cpl.").GenderCount["Male"] + "/" + demoTotals["H"].First(x => x.RankName == "Cpl.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals["H"].First(x => x.RankName == "Sgt.").GenderCount["Male"] + "/" + demoTotals["H"].First(x => x.RankName == "Sgt.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals["H"].First(x => x.RankName == "Lt.").GenderCount["Male"] + "/" + demoTotals["H"].First(x => x.RankName == "Lt.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals["H"].First(x => x.RankName == "Capt.").GenderCount["Male"] + "/" + demoTotals["H"].First(x => x.RankName == "Capt.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals["H"].First(x => x.RankName == "Exec").GenderCount["Male"] + "/" + demoTotals["H"].First(x => x.RankName == "Exec").GenderCount["Female"] + "</td>" +
                "</tr>" +
                "</table>");
        }
        
        private Dictionary<string, BundledGenderRankDemoCollectionObject> RecurseForChildComponentDemoTotals(RosterManagerViewModelComponent c, Dictionary<string, BundledGenderRankDemoCollectionObject> demos)
        {
            if (c.Children.Count() > 0)
            {
                foreach(RosterManagerViewModelComponent chld in c.Children)
                {
                    RecurseForChildComponentDemoTotals(chld, demos);
                }
            }
            foreach (Position p in c.Positions)
            {
                foreach (Member m in p.Members)
                {
                    //var race = demos[m.Race.Abbreviation.ToString()];
                    //GenderRankDemoCollectionObject rank = race.First(x => x.RankName == m.Rank.RankShort);
                    //rank.GenderCount[m.Gender.GenderFullName]++;
                    demos[m.Race.Abbreviation.ToString()].Where(x => x.RankName == m.Rank.RankShort).First().addToGenderCount(m.Gender.GenderFullName);
                }
            }
            return demos;
        }

    }
}
