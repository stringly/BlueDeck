using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models.Types
{
    public class GenderRankDemoCollectionObject
    {        
        public string RankName { get; set; }
        public Dictionary<string, int> GenderCount { get; set; }

        public GenderRankDemoCollectionObject(string rankName)
        {
            RankName = rankName;
            GenderCount = new Dictionary<string, int>()
            {
                {"Male", 0 },
                {"Female", 0 }
            };
        }
        public void addToGenderCount(string genderName)
        {
            GenderCount[genderName]++;            
        }
    }

    public class BundledGenderRankDemoCollectionObject
    {
        public string RaceAbbreviation {get;set;}
        public List<GenderRankDemoCollectionObject> RankList { get; set; }

        public BundledGenderRankDemoCollectionObject(string race)
        {
            RaceAbbreviation = race;
            RankList = new List<GenderRankDemoCollectionObject>()
            {
                {new GenderRankDemoCollectionObject("P/O")},
                {new GenderRankDemoCollectionObject("POFC")},
                {new GenderRankDemoCollectionObject("Cpl.")},
                {new GenderRankDemoCollectionObject("Sgt.")},
                {new GenderRankDemoCollectionObject("Lt.")},
                {new GenderRankDemoCollectionObject("Capt.")},
                {new GenderRankDemoCollectionObject("Exec")}
            };
        }
        public int TotalGenderCount(string gender)
        {
            int result = 0;
            foreach(GenderRankDemoCollectionObject g in RankList)
            {
                result = result + g.GenderCount[gender];
            }

            return result;
        }
        public int TotalRankCount(string rank, string gender)
        {
            int result = 0;
            foreach(GenderRankDemoCollectionObject g in RankList)
            {
                if (g.RankName == rank)
                {
                    result = result + g.GenderCount[gender];
                }
            }
            return result;
        }
    }
    public class DemoTableDataObject
    {
        List<BundledGenderRankDemoCollectionObject> Bundles {get;set;}

        public DemoTableDataObject(List<BundledGenderRankDemoCollectionObject> bundles)
        {
            Bundles = bundles;
        }
        public int GetRankTotalCount(string rank, string gender)
        {
            int result = 0;
            foreach(BundledGenderRankDemoCollectionObject b in Bundles)
            {
                result = result + b.TotalRankCount(rank, gender);
            }


            return result;
        }
        public int GetTotalGenderCount(string Gender)
        {
            int result = 0;
            foreach(BundledGenderRankDemoCollectionObject b in Bundles)
            {
                result = result + b.TotalGenderCount(Gender);
            }
            return result;
        }
    }
    
}
