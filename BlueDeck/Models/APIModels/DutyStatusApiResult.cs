using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.APIModels
{
    public class DutyStatusApiResult
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public bool HasPolicePower { get; set; }

        public DutyStatusApiResult()
        {
        }

        public DutyStatusApiResult(DutyStatus _dutyStatus)
        {
            Name = _dutyStatus.DutyStatusName;
            Abbreviation = _dutyStatus.Abbreviation.ToString();
            HasPolicePower = _dutyStatus.HasPolicePower;
        }
    }
}
