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
        List<GenderRankDemoCollectionObject> RankList { get; set; }

        public BundledGenderRankDemoCollectionObject()
        {
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
    }
    
}
