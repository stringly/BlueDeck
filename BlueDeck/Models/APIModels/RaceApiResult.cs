using System;
using BlueDeck.Models.Enums;

namespace BlueDeck.Models.APIModels
{
    public class RaceApiResult
    {
        public int RaceId {get;set;}
        public string RaceName { get; set; }
        public string Abbreviation { get; set; }

        public RaceApiResult()
        {
        }

        public RaceApiResult(Race _race)
        {
            RaceId = (Int32)_race.MemberRaceId;
            RaceName = _race.MemberRaceFullName;
            Abbreviation = _race.Abbreviation.ToString();
        }
    }
}
