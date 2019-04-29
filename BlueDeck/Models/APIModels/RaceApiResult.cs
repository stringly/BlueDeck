using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.APIModels
{
    public class RaceApiResult
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }

        public RaceApiResult()
        {
        }

        public RaceApiResult(Race _race)
        {
            Name = _race.MemberRaceFullName;
            Abbreviation = _race.Abbreviation.ToString();
        }
    }
}
