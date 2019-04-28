using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models.Types
{
    public class RosterManagerViewModelComponent : Component
    {
        public List<RosterManagerViewModelComponent> Children { get; set; }
        public int NestedLevel { get; set; }

        public RosterManagerViewModelComponent(Component c) : base()
        {
            this.Acronym = c.Acronym;
            this.ComponentId = c.ComponentId;
            this.Name = c.Name;
            this.ParentComponent = c.ParentComponent;
            this.Positions = c?.Positions ?? new List<Position>();
            this.Children = new List<RosterManagerViewModelComponent>();
            this.LineupPosition = c.LineupPosition;
        }

        public int GetNestedChildrenDepth(RosterManagerViewModelComponent parent, int depth = 0)
        {
            if (parent.Children.Any())
            {
                foreach (RosterManagerViewModelComponent c in parent.Children)
                {
                    GetNestedChildrenDepth(c, depth++);
                }
            }
            return depth;
        }

        private List<BundledGenderRankDemoCollectionObject> RecurseForChildComponentDemoTotals(RosterManagerViewModelComponent c, List<BundledGenderRankDemoCollectionObject> demos)
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
                    try
                    {
                        demos.Single(x => x.RaceAbbreviation == m.Race.Abbreviation.ToString()).RankList.Single(x => x.RankName == m.Rank.RankShort).addToGenderCount(m.Gender.GenderFullName);
                    }
                    catch (Exception e)
                    {
                        Console.Write(e.InnerException);
                    }
                }
            }
            return demos;
        }

        public HtmlString GetDemographicTableForComponentAndChildren()
        {  
            List<BundledGenderRankDemoCollectionObject> demoEmpty = new List<BundledGenderRankDemoCollectionObject>(){
                {new BundledGenderRankDemoCollectionObject("B")},
                {new BundledGenderRankDemoCollectionObject("W")},
                {new BundledGenderRankDemoCollectionObject("A")},
                {new BundledGenderRankDemoCollectionObject("I")},
                {new BundledGenderRankDemoCollectionObject("O")},
                {new BundledGenderRankDemoCollectionObject("H")}
            };
            List<BundledGenderRankDemoCollectionObject> demoTotals = RecurseForChildComponentDemoTotals(this, demoEmpty);
            DemoTableDataObject obj = new DemoTableDataObject(demoTotals);
            return new HtmlString( "<table class='componentDemoTable' data-componentId='" + this.ComponentId + "'>" +
                "<tr class='componentDemoTableHeaderRow' >" +
                    "<td colspan=9>" + this.Name + " Demographics" + "</td>" +
                "</tr>" +
                "<tr class='componentDemoTableSubHeaderRow' >" +
                    "<th>" +
                    "<th>P/O" +
                    "<th>POFC" +
                    "<th>Cpl." +
                    "<th>Sgt." +
                    "<th>Lt." +
                    "<th>Capt" +
                    "<th>Exec" +
                    "<th>Total" +
                "</tr>" +
                "<tr class='componentDemoTableSubSubHeaderRow'>" +
                    "<td></td>" +
                    "<td>M/F</td>" +
                    "<td>M/F</td>" +
                    "<td>M/F</td>" +
                    "<td>M/F</td>" +
                    "<td>M/F</td>" +
                    "<td>M/F</td>" +
                    "<td>M/F</td>" +
                    "<td>M/F</td>" +
                "</tr>" +                    
                    "<td class='firstRow'>B </td>" +
                    "<td>" + demoTotals.Single(x => x.RaceAbbreviation == "B").RankList.Single(x => x.RankName == "P/O").GenderCount["Male"] + "/" + demoTotals.Single(x => x.RaceAbbreviation == "B").RankList.Single(x => x.RankName == "P/O").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals.Single(x => x.RaceAbbreviation == "B").RankList.Single(x => x.RankName == "POFC").GenderCount["Male"] + "/" + demoTotals.Single(x => x.RaceAbbreviation == "B").RankList.Single(x => x.RankName == "POFC").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals.Single(x => x.RaceAbbreviation == "B").RankList.Single(x => x.RankName == "Cpl.").GenderCount["Male"] + "/" + demoTotals.Single(x => x.RaceAbbreviation == "B").RankList.Single(x => x.RankName == "Cpl.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals.Single(x => x.RaceAbbreviation == "B").RankList.Single(x => x.RankName == "Sgt.").GenderCount["Male"] + "/" + demoTotals.Single(x => x.RaceAbbreviation == "B").RankList.Single(x => x.RankName == "Sgt.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals.Single(x => x.RaceAbbreviation == "B").RankList.Single(x => x.RankName == "Lt.").GenderCount["Male"] + "/" + demoTotals.Single(x => x.RaceAbbreviation == "B").RankList.Single(x => x.RankName == "Lt.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals.Single(x => x.RaceAbbreviation == "B").RankList.Single(x => x.RankName == "Capt.").GenderCount["Male"] + "/" + demoTotals.Single(x => x.RaceAbbreviation == "B").RankList.Single(x => x.RankName == "Capt.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals.Single(x => x.RaceAbbreviation == "B").RankList.Single(x => x.RankName == "Exec").GenderCount["Male"] + "/" + demoTotals.Single(x => x.RaceAbbreviation == "B").RankList.Single(x => x.RankName == "Exec").GenderCount["Female"] + "</td>" +
                    "<td style='font-weight:bold'>" + demoTotals.Single(x => x.RaceAbbreviation == "B").TotalGenderCount("Male") + "/"  + demoTotals.Single(x => x.RaceAbbreviation == "B").TotalGenderCount("Female") + "</td>" +
                "</tr>" +
                "<tr>" +
                    "<td class='firstRow'>W </td>" +
                    "<td>" + demoTotals.Single(x => x.RaceAbbreviation == "W").RankList.Single(x => x.RankName == "P/O").GenderCount["Male"] + "/" + demoTotals.Single(x => x.RaceAbbreviation == "W").RankList.Single(x => x.RankName == "P/O").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals.Single(x => x.RaceAbbreviation == "W").RankList.Single(x => x.RankName == "POFC").GenderCount["Male"] + "/" + demoTotals.Single(x => x.RaceAbbreviation == "W").RankList.Single(x => x.RankName == "POFC").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals.Single(x => x.RaceAbbreviation == "W").RankList.Single(x => x.RankName == "Cpl.").GenderCount["Male"] + "/" + demoTotals.Single(x => x.RaceAbbreviation == "W").RankList.Single(x => x.RankName == "Cpl.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals.Single(x => x.RaceAbbreviation == "W").RankList.Single(x => x.RankName == "Sgt.").GenderCount["Male"] + "/" + demoTotals.Single(x => x.RaceAbbreviation == "W").RankList.Single(x => x.RankName == "Sgt.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals.Single(x => x.RaceAbbreviation == "W").RankList.Single(x => x.RankName == "Lt.").GenderCount["Male"] + "/" + demoTotals.Single(x => x.RaceAbbreviation == "W").RankList.Single(x => x.RankName == "Lt.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals.Single(x => x.RaceAbbreviation == "W").RankList.Single(x => x.RankName == "Capt.").GenderCount["Male"] + "/" + demoTotals.Single(x => x.RaceAbbreviation == "W").RankList.Single(x => x.RankName == "Capt.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals.Single(x => x.RaceAbbreviation == "W").RankList.Single(x => x.RankName == "Exec").GenderCount["Male"] + "/" + demoTotals.Single(x => x.RaceAbbreviation == "W").RankList.Single(x => x.RankName == "Exec").GenderCount["Female"] + "</td>" +
                    "<td style='font-weight:bold'>" + demoTotals.Single(x => x.RaceAbbreviation == "W").TotalGenderCount("Male") + "/"  + demoTotals.Single(x => x.RaceAbbreviation == "W").TotalGenderCount("Female") + "</td>" +
                "</tr>" +
                "<tr>" +
                    "<td class='firstRow'>A </td>" +
                    "<td>" + demoTotals.Single(x => x.RaceAbbreviation == "A").RankList.Single(x => x.RankName == "P/O").GenderCount["Male"] + "/" + demoTotals.Single(x => x.RaceAbbreviation == "A").RankList.Single(x => x.RankName == "P/O").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals.Single(x => x.RaceAbbreviation == "A").RankList.Single(x => x.RankName == "POFC").GenderCount["Male"] + "/" + demoTotals.Single(x => x.RaceAbbreviation == "A").RankList.Single(x => x.RankName == "POFC").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals.Single(x => x.RaceAbbreviation == "A").RankList.Single(x => x.RankName == "Cpl.").GenderCount["Male"] + "/" + demoTotals.Single(x => x.RaceAbbreviation == "A").RankList.Single(x => x.RankName == "Cpl.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals.Single(x => x.RaceAbbreviation == "A").RankList.Single(x => x.RankName == "Sgt.").GenderCount["Male"] + "/" + demoTotals.Single(x => x.RaceAbbreviation == "A").RankList.Single(x => x.RankName == "Sgt.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals.Single(x => x.RaceAbbreviation == "A").RankList.Single(x => x.RankName == "Lt.").GenderCount["Male"] + "/" + demoTotals.Single(x => x.RaceAbbreviation == "A").RankList.Single(x => x.RankName == "Lt.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals.Single(x => x.RaceAbbreviation == "A").RankList.Single(x => x.RankName == "Capt.").GenderCount["Male"] + "/" + demoTotals.Single(x => x.RaceAbbreviation == "A").RankList.Single(x => x.RankName == "Capt.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals.Single(x => x.RaceAbbreviation == "A").RankList.Single(x => x.RankName == "Exec").GenderCount["Male"] + "/" + demoTotals.Single(x => x.RaceAbbreviation == "A").RankList.Single(x => x.RankName == "Exec").GenderCount["Female"] + "</td>" +
                    "<td style='font-weight:bold'>" + demoTotals.Single(x => x.RaceAbbreviation == "A").TotalGenderCount("Male") + "/"  + demoTotals.Single(x => x.RaceAbbreviation == "A").TotalGenderCount("Female") + "</td>" +
                "</tr>" +
                "<tr>" +
                    "<td class='firstRow'>I </td>" +
                    "<td>" + demoTotals.Single(x => x.RaceAbbreviation == "I").RankList.Single(x => x.RankName == "P/O").GenderCount["Male"] + "/" + demoTotals.Single(x => x.RaceAbbreviation == "I").RankList.Single(x => x.RankName == "P/O").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals.Single(x => x.RaceAbbreviation == "I").RankList.Single(x => x.RankName == "POFC").GenderCount["Male"] + "/" + demoTotals.Single(x => x.RaceAbbreviation == "I").RankList.Single(x => x.RankName == "POFC").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals.Single(x => x.RaceAbbreviation == "I").RankList.Single(x => x.RankName == "Cpl.").GenderCount["Male"] + "/" + demoTotals.Single(x => x.RaceAbbreviation == "I").RankList.Single(x => x.RankName == "Cpl.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals.Single(x => x.RaceAbbreviation == "I").RankList.Single(x => x.RankName == "Sgt.").GenderCount["Male"] + "/" + demoTotals.Single(x => x.RaceAbbreviation == "I").RankList.Single(x => x.RankName == "Sgt.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals.Single(x => x.RaceAbbreviation == "I").RankList.Single(x => x.RankName == "Lt.").GenderCount["Male"] + "/" + demoTotals.Single(x => x.RaceAbbreviation == "I").RankList.Single(x => x.RankName == "Lt.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals.Single(x => x.RaceAbbreviation == "I").RankList.Single(x => x.RankName == "Capt.").GenderCount["Male"] + "/" + demoTotals.Single(x => x.RaceAbbreviation == "I").RankList.Single(x => x.RankName == "Capt.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals.Single(x => x.RaceAbbreviation == "I").RankList.Single(x => x.RankName == "Exec").GenderCount["Male"] + "/" + demoTotals.Single(x => x.RaceAbbreviation == "I").RankList.Single(x => x.RankName == "Exec").GenderCount["Female"] + "</td>" +
                    "<td style='font-weight:bold'>" + demoTotals.Single(x => x.RaceAbbreviation == "I").TotalGenderCount("Male") + "/"  + demoTotals.Single(x => x.RaceAbbreviation == "I").TotalGenderCount("Female") + "</td>" +
                "</tr>" +
                "<tr>" +
                    "<td class='firstRow'>H </td>" +
                    "<td>" + demoTotals.Single(x => x.RaceAbbreviation == "H").RankList.Single(x => x.RankName == "P/O").GenderCount["Male"] + "/" + demoTotals.Single(x => x.RaceAbbreviation == "H").RankList.Single(x => x.RankName == "P/O").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals.Single(x => x.RaceAbbreviation == "H").RankList.Single(x => x.RankName == "POFC").GenderCount["Male"] + "/" + demoTotals.Single(x => x.RaceAbbreviation == "H").RankList.Single(x => x.RankName == "POFC").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals.Single(x => x.RaceAbbreviation == "H").RankList.Single(x => x.RankName == "Cpl.").GenderCount["Male"] + "/" + demoTotals.Single(x => x.RaceAbbreviation == "H").RankList.Single(x => x.RankName == "Cpl.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals.Single(x => x.RaceAbbreviation == "H").RankList.Single(x => x.RankName == "Sgt.").GenderCount["Male"] + "/" + demoTotals.Single(x => x.RaceAbbreviation == "H").RankList.Single(x => x.RankName == "Sgt.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals.Single(x => x.RaceAbbreviation == "H").RankList.Single(x => x.RankName == "Lt.").GenderCount["Male"] + "/" + demoTotals.Single(x => x.RaceAbbreviation == "H").RankList.Single(x => x.RankName == "Lt.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals.Single(x => x.RaceAbbreviation == "H").RankList.Single(x => x.RankName == "Capt.").GenderCount["Male"] + "/" + demoTotals.Single(x => x.RaceAbbreviation == "H").RankList.Single(x => x.RankName == "Capt.").GenderCount["Female"] + "</td>" +
                    "<td>" + demoTotals.Single(x => x.RaceAbbreviation == "H").RankList.Single(x => x.RankName == "Exec").GenderCount["Male"] + "/" + demoTotals.Single(x => x.RaceAbbreviation == "H").RankList.Single(x => x.RankName == "Exec").GenderCount["Female"] + "</td>" +
                    "<td style='font-weight:bold'>" + demoTotals.Single(x => x.RaceAbbreviation == "H").TotalGenderCount("Male") + "/"  + demoTotals.Single(x => x.RaceAbbreviation == "H").TotalGenderCount("Female") + "</td>" +
                "</tr>" +
                "<tr>" +
                    "<td class='firstRow'>Total</td>" +
                    "<td style='font-weight:bold'>" + obj.GetRankTotalCount("P/O", "Male") + "/" + obj.GetRankTotalCount("P/O", "Female") + "</td>" +
                    "<td style='font-weight:bold'>" + obj.GetRankTotalCount("POFC", "Male") + "/" + obj.GetRankTotalCount("POFC", "Female") + "</td>" +
                    "<td style='font-weight:bold'>" + obj.GetRankTotalCount("Cpl.", "Male") + "/" + obj.GetRankTotalCount("Cpl.", "Female") + "</td>" +
                    "<td style='font-weight:bold'>" + obj.GetRankTotalCount("Sgt.", "Male") + "/" + obj.GetRankTotalCount("Sgt.", "Female") + "</td>" +
                    "<td style='font-weight:bold'>" + obj.GetRankTotalCount("Lt.", "Male") + "/" + obj.GetRankTotalCount("Lt.", "Female") + "</td>" +
                    "<td style='font-weight:bold'>" + obj.GetRankTotalCount("Capt.", "Male") + "/" + obj.GetRankTotalCount("Capt.", "Female") + "</td>" +
                    "<td style='font-weight:bold'>" + obj.GetRankTotalCount("Exec", "Male") + "/" + obj.GetRankTotalCount("Exec", "Female") + "</td>" +
                    "<td style='font-weight:bold'>" + obj.GetTotalGenderCount("Male") + "/" + obj.GetTotalGenderCount("Female") + "</td>" +

                "</tr>" +
                "</table>");
        }
    }
}
