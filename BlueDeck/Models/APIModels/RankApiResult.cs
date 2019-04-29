using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.APIModels
{
    public class RankApiResult
    {
        public string RankName { get; set; }
        public string RankShort { get; set; }
        public string PayGrade { get; set; }
        public bool IsSworn { get; set; }

        public RankApiResult()
        {
        }

        public RankApiResult(Rank _rank)
        {
            RankName = _rank.RankFullName;
            RankShort = _rank.RankShort;
            PayGrade = _rank.PayGrade;
            IsSworn = _rank.IsSworn;
        }
    }
}
